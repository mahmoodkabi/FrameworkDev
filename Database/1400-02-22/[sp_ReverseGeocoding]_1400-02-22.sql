USE [OSM]
GO
/****** Object:  StoredProcedure [dbo].[sp_ReverseGeocoding]    Script Date: 22/02/1400 09:36:24 ق.ظ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Runs breadth-first search from a specific node.
-- @StartNode: If of node to start the search at.
-- @EndNode: Stop the search when node with this id is found. Specify NULL
-- to traverse the whole graph.

-- [sp_ReverseGeocoding] '51.647 32.678' 
-- [sp_ReverseGeocoding] '51.641 32.685' 
-- [sp_ReverseGeocoding] '51.638 32.671' 
-- [sp_ReverseGeocoding] '51.68 32.647' 
-- [sp_ReverseGeocoding] '51.675 32.647' 
-- [sp_ReverseGeocoding] '51.701 32.664' 
-- [sp_ReverseGeocoding] '51.412 35.747' 
-- [sp_ReverseGeocoding] '51.405 35.736' 
-- [sp_ReverseGeocoding] '52.318 32.641' 
-- [sp_ReverseGeocoding] '51.673 32.654' 
-- [sp_ReverseGeocoding] '54.3689521874908 31.8896430132097' 
-- exec [dbo].[sp_ReverseGeocoding] @XY='51.785 32.488'   ***
-- exec [dbo].[sp_ReverseGeocoding] @XY='51/8581782836256 32/0242862712363'


ALTER PROCEDURE [dbo].[sp_ReverseGeocoding] (@XY varchar(50))
AS

BEGIN
	--select cast('' as nvarchar(max)) as Address
 
    SET XACT_ABORT ON   
    SET NOCOUNT ON;

	IF OBJECT_ID('tempdb.dbo.#Discovered', 'U') IS NOT NULL
		DROP TABLE #Discovered; 

	IF OBJECT_ID('tempdb.dbo.#County', 'U') IS NOT NULL
		DROP TABLE #County; 

	declare @BufferXYStartNode geometry, @BufferXYEndNode geometry, @XYGeometry geometry, @BufferXYSearchRange geometry
	declare @StartNode nvarchar(10), @EndNode nvarchar(10), @tempNode nvarchar(10)
	declare @IdDiscovered int, @NameDiscovered nvarchar(500), @OrderDiscovered int, @PathDiscovered nvarchar(1000), @NamePathDiscovered nvarchar(Max) 
	declare @tbl table(id int identity, StreetName nvarchar(500))
	declare @Reorderaddress nvarchar(Max), @StreetName nvarchar(500)
	declare @CountyCityZone nvarchar(150)
	declare @CityGeometry geometry, @CityOsmId nvarchar(10), @CityName nvarchar(100)
	declare @CountyGeometry geometry, @CountyOsmId nvarchar(10), @CountyName nvarchar(100)
	declare @ZoneGeometry geometry, @ZoneOsmId nvarchar(10), @ZoneName nvarchar(100)
	declare @PointOfInterrest nvarchar(100)

	set @XY = REPLACE(@XY,'/', '.')
	set @XY = REPLACE(@XY,'/', '.')

	--Convert xy string to geometry
	select @XYGeometry =  geometry::STGeomFromText('POINT('+@XY+')',4326) 

	--Add a buffer to the point of origin to find the nearest starting passage
	select @BufferXYStartNode =  geometry::STGeomFromText('POINT('+@XY+')',4326).STBuffer(0.001)

	--Add a buffer to the point of origin to find the nearest ending passage
	select @BufferXYEndNode =  geometry::STGeomFromText('POINT('+@XY+')',4326).STBuffer(0.01) 



	-- find county and city and zone
	select top 1 @CountyGeometry = place_a.Shape, @CountyOsmId = place_a.osm_id, @CountyName = place_a.name from OSM_PLACES_A place_a where place_a.fclass ='county' and @XYGeometry.STIntersects(place_a.Shape) = 1
	select top 1 @CityGeometry = place_a.Shape, @CityOsmId = place_a.osm_id, @CityName = place_a.name from OSM_PLACES_A place_a where place_a.fclass in('city', 'town', 'island', 'national_capital') and @XYGeometry.STIntersects(place_a.Shape) = 1
	select top 1 @ZoneGeometry = place_a.Shape, @ZoneOsmId = place_a.osm_id, @ZoneName = place_a.name from OSM_PLACES_A place_a where place_a.fclass ='suburb' and @XYGeometry.STIntersects(place_a.Shape) = 1
	if isnull(@CityName,'') != '' and isnull(@ZoneName,'') != ''
		set @CountyCityZone = isnull(@CountyName,'') + ' - ' + N' شهر' +  isnull(@CityName,'') + ' - ' + isnull(@ZoneName,'')
	else if isnull(@CityName,'') != ''
		set @CountyCityZone = isnull(@CountyName,'') + ' - ' + isnull(@CityName,'')
	else if isnull(@ZoneName,'') != ''
		set @CountyCityZone = isnull(@CountyName,'') + ' - ' + isnull(@ZoneName,'')
	else
		set @CountyCityZone = isnull(@CountyName,'') 

	print @CountyName
	print @ZoneName
	print @CountyCityZone
	
	--محدود كردن جستجو به شهرستان
	Select *  Into #County from OSM_ROADS where CountyOsmId = @CountyOsmId

	--پيدا كردن پوينت آف اينترست
	select top 1 @PointOfInterrest = place.name from OSM_PLACES place  where CountyOsmId = @CountyOsmId and @BufferXYEndNode.STIntersects(place.Shape) = 1 and place.fclass in('village', 'town') order by @XYGeometry.STDistance(place.SHAPE) asc
	if isnull(@PointOfInterrest ,'') != ''
		set @CountyCityZone = isnull(@CountyCityZone,'') + ' - ' + isnull(@PointOfInterrest,'')


	---------------------Start point---------------------------------------------------------
	-----------------------------------------------------------------------------------------
	--جستجو در شهر
	select top 1 @StartNode = A.osm_id from (
		select Road.osm_id, Road.Shape from OSM_ROADS Road where @CityGeometry.STIntersects(Road.Shape) = 1 
	) A where  @BufferXYStartNode.STIntersects(A.SHAPE)=1  order by @XYGeometry.STDistance(A.SHAPE) asc 

	--جستجو در شهرستان
	if(isnull(@StartNode, '0') = '0')
	begin
		select top 1 @StartNode = isnull(A.osm_id,'0') from (
			select Road.osm_id, Road.Shape from OSM_ROADS Road where @CountyGeometry.STIntersects(Road.Shape) = 1 
		) A where  @BufferXYStartNode.STIntersects(A.SHAPE)=1  order by @XYGeometry.STDistance(A.SHAPE) asc 
	end
	print isnull(@StartNode,'null') + ' 11111111111'

	--------------------------End Point----------------------------------------------------------
	------------------جستجو در شهر--------------------------------------------------------------
	select top 1 @EndNode = A.osm_id from  (
		select Road.osm_id, Road.Shape, Road.fclass, Road.name from OSM_ROADS Road where @CityGeometry.STIntersects(Road.Shape) = 1 
	) A where (A.fclass ='primary' or A.fclass ='trunk') and isnull(A.name, '') != '' and @BufferXYEndNode.STIntersects(A.SHAPE)=1 order by @XYGeometry.STDistance(A.SHAPE) asc 
	--در صورتي كه نقطه انتهايي نال بود، بافر را براي نقطه پاياني بزرگتر مي كنيم
	if isnull(@EndNode, '') = ''
	begin
		select @BufferXYEndNode =  geometry::STGeomFromText('POINT('+@XY+')',4326).STBuffer(0.1) 

		select top 1 @EndNode = A.osm_id from  (
		select Road.osm_id, Road.Shape, Road.fclass, Road.name  from OSM_ROADS Road where @CityGeometry.STIntersects(Road.Shape) = 1 
	) A where (A.fclass ='secondary' or A.fclass ='primary' or A.fclass ='trunk') and isnull(A.name, '') != '' and @BufferXYEndNode.STIntersects(A.SHAPE)=1 order by @XYGeometry.STDistance(A.SHAPE) asc 
	end

	----------------جستجو در شهرستان----------------------------------------------------------------
	if(isnull(@EndNode, '0') = '0')
	begin
		select @BufferXYEndNode =  geometry::STGeomFromText('POINT('+@XY+')',4326).STBuffer(0.01) 

		select top 1 @EndNode = A.osm_id from  (
			select Road.osm_id, Road.Shape, Road.fclass, Road.name  from OSM_ROADS Road where @CountyGeometry.STIntersects(Road.Shape) = 1 
		) A where (A.fclass ='secondary' or A.fclass ='primary' or A.fclass ='trunk' ) and isnull(A.name, '') != '' and @BufferXYEndNode.STIntersects(A.SHAPE)=1 order by @XYGeometry.STDistance(A.SHAPE) asc 
	end
	--در صورتي كه نقطه انتهايي نال بود، بافر را براي نقطه پاياني بزرگتر مي كنيم
	if isnull(@EndNode, '') = ''
	begin
		select @BufferXYEndNode =  geometry::STGeomFromText('POINT('+@XY+')',4326).STBuffer(0.1) 

		select top 1 @EndNode = A.osm_id from  (
			select Road.osm_id, Road.Shape, Road.fclass, Road.name  from OSM_ROADS Road where @CountyGeometry.STIntersects(Road.Shape) = 1 
		) A where (A.fclass ='secondary' or A.fclass ='primary' or A.fclass ='trunk' ) and isnull(A.name, '') != '' and @BufferXYEndNode.STIntersects(A.SHAPE)=1 order by @XYGeometry.STDistance(A.SHAPE) asc 
		
		
		--select * from  (
		--	select Road.osm_id, Road.Shape, Road.fclass from OSM_ROADS Road where @CountyGeometry.STIntersects(Road.Shape) = 1 
		--) A where (A.fclass ='secondary' or A.fclass ='primary' or A.fclass ='trunk' )  and @BufferXYEndNode.STIntersects(A.SHAPE)=1 order by @XYGeometry.STDistance(A.SHAPE) asc 

	end


	print isnull(@EndNode, 'null') + ' 22222222222'



	
    -- Create a temporary table for storing the discovered nodes as the algorithm runs
    CREATE TABLE #Discovered
    (
        Id int NOT NULL PRIMARY KEY,    -- The Node Id
        Predecessor int NULL,    -- The node we came from to get to this node.
        OrderDiscovered int -- The order in which the nodes were discovered.
    )

    -- Initially, only the start node is discovered.
    INSERT INTO #Discovered (Id, Predecessor, OrderDiscovered)
    VALUES (@StartNode, NULL, 0)


    -- Add all nodes that we can get to from the current set of nodes,
    -- that are not already discovered. Run until no more nodes are discovered.
    WHILE @@ROWCOUNT > 0
    BEGIN

        -- If we have found the node we were looking for, abort now.
        IF @EndNode IS NOT NULL
            IF EXISTS (SELECT TOP 1 1 FROM #Discovered WHERE Id = @EndNode)
                BREAK   

        -- We need to group by ToNode and select one FromNode since multiple
        -- edges could lead us to new same node, and we only want to insert it once.
        INSERT INTO #Discovered (Id, Predecessor, OrderDiscovered)
        SELECT e.ToNode, MIN(e.FromNode), MIN(d.OrderDiscovered) + 1
        FROM #Discovered d JOIN (Select * from IranNode where CountyOsmId = @CountyOsmId) e ON d.Id = e.FromNode
        WHERE e.ToNode NOT IN (SELECT Id From #Discovered)
        GROUP BY e.ToNode
    END;

   
    -- Select the results. We use a recursive common table expression to
    -- get the full path from the start node to the current node.
    WITH BacktraceCTE(Id, Name, OrderDiscovered, Path, NamePath)
    AS
    (
        -- Anchor/base member of the recursion, this selects the start node.
        SELECT n.osm_id Id, n.name Name, d.OrderDiscovered, CAST(n.osm_id AS varchar(MAX)),
            CAST(n.name AS nvarchar(MAX))
        FROM #Discovered d JOIN #County n ON d.Id = n.osm_id
        WHERE d.Id = @StartNode

        UNION ALL

        -- Recursive member, select all the nodes which have the previous
        -- one as their predecessor. Concat the paths together.
        SELECT n.osm_id Id, n.name Name, d.OrderDiscovered,
            CAST(cte.Path + ',' + CAST(n.osm_id as varchar(10)) as varchar(MAX)),
            cte.NamePath + ',' + n.name Name
        FROM #Discovered d JOIN BacktraceCTE cte ON d.Predecessor = cte.Id
        JOIN #County n ON d.Id = n.osm_id
    )
		
    SELECT @IdDiscovered =  Id, @NameDiscovered =  Name, @OrderDiscovered =  OrderDiscovered, @PathDiscovered =  Path, @NamePathDiscovered =  NamePath FROM BacktraceCTE
    WHERE Id = @EndNode OR @EndNode IS NULL -- This kind of where clause can potentially produce
    ORDER BY OrderDiscovered                -- a bad execution plan, but I use it for simplicity here.

	print '99999999999999999999999999999999999'
	print @NamePathDiscovered + ' 00 55555555555 999 6666'
	print @PathDiscovered + ' 11'

	insert into @tbl(StreetName)
	select * from  dbo.SplitString(@NamePathDiscovered, ',')


	-------Reorder Address
	declare CursorAddress cursor FAST_FORWARD FOR 
	select t.StreetName from @tbl t order by t.id desc

	open CursorAddress
	fetch next from CursorAddress
	into @StreetName

	set @Reorderaddress = @CountyCityZone
	while @@FETCH_STATUS = 0
	begin
		print isnull(@StreetName,'')
		set @Reorderaddress = isnull(@Reorderaddress,'') + ' - ' + isnull(@StreetName,'')

		fetch next from CursorAddress
		into @StreetName
	end

	CLOSE CursorAddress;  
	DEALLOCATE CursorAddress; 
	
	set  @Reorderaddress = REPLACE(@Reorderaddress, '--', '-') 
	set  @Reorderaddress = REPLACE(@Reorderaddress, '--', '-') 
	set  @Reorderaddress = REPLACE(@Reorderaddress, '--', '-') 
	set  @Reorderaddress = REPLACE(@Reorderaddress, '--', '-') 
	set  @Reorderaddress = REPLACE(@Reorderaddress, '--', '-') 
	set  @Reorderaddress = REPLACE(@Reorderaddress, '- -', '-') 
	set  @Reorderaddress = REPLACE(@Reorderaddress, '- -', '-') 
	set  @Reorderaddress = REPLACE(@Reorderaddress, '- -', '-') 
	set  @Reorderaddress = REPLACE(@Reorderaddress, '- -', '-') 
	set  @Reorderaddress = REPLACE(@Reorderaddress, '- -', '-') 
	select @Reorderaddress as Address

    DROP TABLE #Discovered
	Drop Table #County

----select  A.osm_id FromNode, B.osm_id ToNode, B.code, B.fclass  
----into IranNode 
----from
----(select * from dbo.OSM_ROADS N) A join
----(select * from dbo.OSM_ROADS N) B  on 1=1
----where  A.SHAPE.STBuffer(0.00001).STIntersects(B.SHAPE)=1 and A.OBJECTID != B.OBJECTID

END
