USE [OSM]
GO
/****** Object:  StoredProcedure [dbo].[sp_GeoCoding]    Script Date: 5/6/2021 6:15:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mahmood Kabi
-- Create date: 1400-02-10
-- Description: تبديل آدرس به طول و عرض جغرافيايي          
-- =============================================

--[dbo].[sp_GeoCoding]   @County = N'اصفهان', @City = N'اصفهان', @Zone = N'منطقه ۳', @Mabar1 = N'خیابان فردوسی', @Mabar2 =N'', @Mabar3 =N''
--[dbo].[sp_GeoCoding]   @County = N'اصفهان', @City = N'اصفهان', @Zone = N'', @Mabar1 = N'خیابان فردوسی', @Mabar2 =N'', @Mabar3 =N''
--[dbo].[sp_GeoCoding]   @County = N'اصفهان', @City = N'اصفهان', @Zone = N'', @Mabar1 = N'فردوسی', @Mabar2 =N'', @Mabar3 =N''
--[dbo].[sp_GeoCoding]   @County = N'اصفهان', @City = N'اصفهان', @Zone = N'', @Mabar1 = N'فردوسی', @Mabar2 =N'غفار', @Mabar3 =N''
--[dbo].[sp_GeoCoding]   @County = N'اصفهان', @City = N'اصفهان', @Zone = N'', @Mabar1 = N'فردوسی', @Mabar2 =N'غفار', @Mabar3 =N''
--[dbo].[sp_GeoCoding]   @County = N'اصفهان', @City = N'اصفهان', @Zone = N'', @Mabar1 = N'پشت بارو', @Mabar2 =N'', @Mabar3 =N''


ALTER proc [dbo].[sp_GeoCoding]  @County nvarchar(100) = '', @City nvarchar(100) = '', @Zone nvarchar(100) = '',  @Mabar1 nvarchar(100), @Mabar2 nvarchar(100), @Mabar3 nvarchar(100)
as 
begin

--select cast('' as nvarchar(200)) as Point

SET XACT_ABORT ON   
SET NOCOUNT ON;

IF OBJECT_ID('tempdb.dbo.#tblResultB', 'U') IS NOT NULL
	DROP TABLE #tblResultB; 

IF OBJECT_ID('tempdb.dbo.#tblResultA', 'U') IS NOT NULL
	DROP TABLE #tblResultA; 

IF OBJECT_ID('tempdb.dbo.#tblResultIf_A_B_IsEmpty', 'U') IS NOT NULL
	DROP TABLE #tblResultIf_A_B_IsEmpty; 

IF OBJECT_ID('tempdb.dbo.#County', 'U') IS NOT NULL
	DROP TABLE #County; 

IF OBJECT_ID('tempdb.dbo.#City', 'U') IS NOT NULL
	DROP TABLE #City; 


declare @tbl table(id int identity, StreetName nvarchar(500))
declare @StreetName nvarchar(500), @Address nvarchar(Max)
declare @Mabar4 nvarchar(100), @Mabar5 nvarchar(100), @Mabar6 nvarchar(100)
declare @i int, @strPoint nvarchar(200)
declare @ResultShape geometry
create table #tblResultB(id int identity, name nvarchar(500), Shape geometry, osm_id int, fclass nvarchar(28))
create table #tblResultA(id int identity, name nvarchar(500), Shape geometry, osm_id int, fclass nvarchar(28))
create table #tblResultIf_A_B_IsEmpty(id int identity, name nvarchar(500), Shape geometry, osm_id int, fclass nvarchar(28))
declare @CityGeometry geometry, @CityOsmId nvarchar(10), @CityName nvarchar(100)
declare @CountyGeometry geometry, @CountyOsmId nvarchar(10), @CountyName nvarchar(100)
create table #LemitedResultOfRoad (OBJECTID int, osm_id nvarchar(10), code smallint, fclass nvarchar(28), name nvarchar(100), ref nvarchar(20), oneway nvarchar(1), maxspeed smallint, layer numeric(12,0), bridge nvarchar(1), tunnel nvarchar(1), Shape Geometry, CountyOsmId nvarchar(10), CityOsmId nvarchar(10))
create table #County (OBJECTID int, osm_id nvarchar(10), code smallint, fclass nvarchar(28), name nvarchar(100), ref nvarchar(20), oneway nvarchar(1), maxspeed smallint, layer numeric(12,0), bridge nvarchar(1), tunnel nvarchar(1), Shape Geometry, CountyOsmId nvarchar(10), CityOsmId nvarchar(10))
create table #City (OBJECTID int, osm_id nvarchar(10), code smallint, fclass nvarchar(28), name nvarchar(100), ref nvarchar(20), oneway nvarchar(1), maxspeed smallint, layer numeric(12,0), bridge nvarchar(1), tunnel nvarchar(1), Shape Geometry, CountyOsmId nvarchar(10), CityOsmId nvarchar(10))


-- جستجو بر اساس آدرس
set @County = ltrim(Rtrim(isnull(@County, '')))
set @City = ltrim(Rtrim(isnull(@City, '')))
set @Zone = ltrim(Rtrim(isnull(@Zone, '')))
set @Mabar1 = ltrim(Rtrim(isnull(@Mabar1, '')))
set @Mabar2 = ltrim(Rtrim(isnull(@Mabar2, '')))
set @Mabar3 = ltrim(Rtrim(isnull(@Mabar3, '')))
set @Mabar4 = ltrim(Rtrim(isnull(@Mabar4, '')))
set @Mabar5 = ltrim(Rtrim(isnull(@Mabar5, '')))
set @Mabar6 = ltrim(Rtrim(isnull(@Mabar6, '')))
set @Address = @County + '-' + @City+ '-' + @Zone + '-' + @Mabar1 + '-' + @Mabar2 + '-' + @Mabar3 + '-' + @Mabar4 + '-' + @Mabar5 + '-' + @Mabar6


--مرحله Text Processing
set @Address = REPLACE(@Address , NCHAR(1610), NCHAR(1740)) 
set @Address = REPLACE(@Address , NCHAR(1603), NCHAR(1705))
set @Address = REPLACE(@Address,N'،','')
set @Address = REPLACE(@Address,N'استان ','')
set @Address = REPLACE(@Address,N'شهرستان ','')
set @Address = REPLACE(@Address,N'شهر ','')
set @Address = REPLACE(@Address,N'بزرگراه ','')
set @Address = REPLACE(@Address,N'بلوار ','')
set @Address = REPLACE(@Address,N'اتوبان ','')
set @Address = REPLACE(@Address,N'منطقه ','')
set @Address = REPLACE(@Address,N'محله ','')
set @Address = REPLACE(@Address,N'خیابان ','')
set @Address = REPLACE(@Address,N'چهارراه ','')
set @Address = REPLACE(@Address,N'جنب ','')
set @Address = REPLACE(@Address,N'کوچه ','')
set @Address = REPLACE(@Address,N'بن بست','')
set @Address = REPLACE(@Address,'_','')
set @Address = REPLACE(@Address,N'کوی ','')
set @Address = REPLACE(@Address,N'میدان ','')
set @Address = REPLACE(@Address,N'پلاک ','')
set @Address = REPLACE(@Address,N'بلوک ','')
set @Address = REPLACE(@Address,N'ورودی ','')
set @Address = REPLACE(@Address,N'طبقه ','')
set @Address = REPLACE(@Address,N'مجتمع ','')
set @Address = REPLACE(@Address,N'بلوار ','')
set @Address = REPLACE(@Address,N'فرعی ','')
set @Address = REPLACE(@Address,N'سمت ','')
set @Address = REPLACE(@Address,N'چپ ','')
set @Address = REPLACE(@Address,N'راست ','')
set @Address = REPLACE(@Address,N'منزل ','')
set @Address = REPLACE(@Address,N'کوی ','')




insert into @tbl(StreetName)
select * from  dbo.SplitString(@Address, '-')

set @County = isnull((select t.StreetName from @tbl t where t.id = 1),'')
set @City = isnull((select t.StreetName from @tbl t where t.id = 2),'')
set @Zone = isnull((select t.StreetName from @tbl t where t.id = 3),'')
set @Mabar1 = isnull((select t.StreetName from @tbl t where t.id = 4),'')
set @Mabar2 = isnull((select t.StreetName from @tbl t where t.id = 5),'')
set @Mabar3 = isnull((select t.StreetName from @tbl t where t.id = 5),'')
set @Mabar4 = isnull((select t.StreetName from @tbl t where t.id = 6),'')
set @Mabar5 = isnull((select t.StreetName from @tbl t where t.id = 7),'')
set @Mabar6 = isnull((select t.StreetName from @tbl t where t.id = 8),'')


--select @Province, @Zone, @Mahaleh, @Mabar1,@Mabar2, @Mabar3, @Mabar4, @Mabar5, @Mabar6


--------------------------------------------------محدود كردن جستجو-----------------------------------------------
if(@County != '')
begin
	select top 1 @CountyGeometry = place_a.Shape, @CountyOsmId = place_a.osm_id, @CountyName = place_a.name from OSM_PLACES_A place_a where place_a.fclass ='county' and place_a.name like N'%' + @County + '%'

	--محدود كردن جستجو به شهرستان
	insert into #County(OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId)
	Select  OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId from OSM_ROADS road where CountyOsmId = @CountyOsmId
end

if(@City != '')
	select top 1 @CityGeometry = place_a.Shape, @CityOsmId = place_a.osm_id, @CityName = place_a.name from OSM_PLACES_A place_a where place_a.fclass in('city', 'town', 'island') and  place_a.name like N'%' + @City + '%'

--اگر شهرستان و شهر مقدار داشت
if (select count(*) from #County) > 0   and @City != '' 
begin
	--محدود كردن جستجو به شهر
	insert into #City(OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId)
	Select  OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId  from #County county where CityOsmId = @CityOsmId

	--Select * Into #City  from #County county where CityOsmId = @CityOsmId
end
else if @City != ''  --اگر تنها شهر مقدار داشت 
begin
	insert into #City(OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId)
	Select  OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId  from OSM_ROADS  where CityOsmId = @CityOsmId
end

----------------------------------------در شهر جستجو انجام شود و يا در شهرستان و يا در كل كشور-----------------------------------------
if(select count(*) from #City) > 0
begin
	insert into #LemitedResultOfRoad(OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId)
	select OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId 
	from #City

	insert into #tblResultA(name, osm_id, Shape, fclass)
	select @CityName, @CityOsmId, @CityGeometry, 'City'

end
else if(select count(*) from #County) > 0
begin
	insert into #LemitedResultOfRoad(OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId)
	select OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId 
	from #County

	insert into #tblResultA(name, osm_id, Shape, fclass)
	select @CountyName, @CountyOsmId, @CountyGeometry, 'County'
end
else
begin
	insert into #LemitedResultOfRoad(OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId)
	select OBJECTID, osm_id, code, fclass, name, ref, oneway, maxspeed, layer, bridge, tunnel, Shape, CountyOsmId, CityOsmId 
	from OSM_ROADS

	insert into #tblResultA(name, osm_id, Shape, fclass)
	select N'ايران', 0, @CountyGeometry, 'AllCountry'
end

-----------------------------------------------------------------------------------------------------

print @Zone + ' zone'

if (select count(*) from #tblResultA) > 0 and @Zone != '' 
begin
	insert into #tblResultB(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass from #LemitedResultOfRoad I join 
	OSM_PLACES_A R on  R.SHAPE.STIntersects(I.Shape)=1  and R.name like N'%' + @Zone + '%'
	--where  (I.fclass ='primary' or I.fclass ='trunk' or I.fclass ='secondary')  

	insert into #tblResultIf_A_B_IsEmpty(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass  from #tblResultB I
	--select I.name, I.osm_id, I.Shape from OSM_ROADS I join 
	--#tblResultA R on 1=1
	--where  I.SHAPE.STIntersects(R.Shape.STBuffer(1))=1 and I.name like N'%' + @Mabar1 + '%'

	delete #tblResultA
end
else 
begin
	insert into #tblResultB(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass from #LemitedResultOfRoad I 
	where  (I.fclass ='primary' or I.fclass ='trunk' or I.fclass ='secondary')  

	insert into #tblResultIf_A_B_IsEmpty(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass  from #tblResultB I
	--select I.name, I.osm_id, I.Shape from OSM_ROADS I join 
	--#tblResultA R on 1=1
	--where  I.SHAPE.STIntersects(R.Shape.STBuffer(1))=1 and I.name like N'%' + @Mabar1 + '%'

	delete #tblResultA
end

-- اگر منطقه مقدار داشت، خیابان های آن منطقه با توجه به نام های اولین معبر در آدرس را جستجو کن
if (select count(*) from #tblResultB) > 0 and @Mabar1 != '' 
begin
	insert into #tblResultA(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass from #LemitedResultOfRoad I join 
	#tblResultB R on  I.SHAPE.STIntersects(R.Shape)=1 and I.name like N'%' + @Mabar1 + '%'

	--[dbo].[sp_GeoCoding]   @County = N'اصفهان', @City = N'اصفهان', @Zone = N'', @Mabar1 = N'خیابان فردوسی', @Mabar2 =N'', @Mabar3 =N''
	--print @Mabar1
	--select * from #tblResultA

	--تمامی معابر پیدا شده در جدول زیر نیز اینسرت می شود
	-- تا در صورتی که دو جدول آ و ب نتیجه ای نداشت از آخرین رکورد این جدول استفاده شود
	insert into #tblResultIf_A_B_IsEmpty(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass  from #tblResultA I
	--select I.name, I.osm_id, I.Shape from OSM_ROADS I join 
	--#tblResultB R on 1=1
	--where  I.SHAPE.STIntersects(R.Shape.STBuffer(1))=1 and I.name like N'%' + @Mabar2 + '%'

	delete #tblResultB
end

-- اگر معبر 1 مقدار داشت، خیابان های متصل به معبر 1 با توجه به نام های دومین معبر در آدرس را جستجو کن
if (select count(*) from #tblResultA) > 0 and @Mabar2 != '' 
begin
	insert into #tblResultB(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass from #LemitedResultOfRoad I join 
	#tblResultA R on I.SHAPE.STIntersects(R.Shape)=1 and I.name like N'%' + @Mabar2 + '%'

	insert into #tblResultIf_A_B_IsEmpty(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass  from #tblResultB I
	--select I.name, I.osm_id, I.Shape from OSM_ROADS I join 
	--#tblResultA R on 1=1
	--where  I.SHAPE.STIntersects(R.Shape.STBuffer(1))=1 and I.name like N'%' + @Mabar3 + '%'

	delete #tblResultA
end


if (select count(*) from #tblResultB) > 0 and @Mabar3 != '' 
begin
	insert into #tblResultA(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass from #LemitedResultOfRoad I join 
	#tblResultB R on   I.SHAPE.STIntersects(R.Shape)=1 and I.name like N'%' + @Mabar3 + '%'

	insert into #tblResultIf_A_B_IsEmpty(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass  from #tblResultA I
	--select I.name, I.osm_id, I.Shape from OSM_ROADS I join 
	--#tblResultB R on 1=1
	--where  I.SHAPE.STIntersects(R.Shape.STBuffer(1))=1 and I.name like N'%' + @Mabar4 + '%'

	delete #tblResultB
end


if (select count(*) from #tblResultA) > 0 and @Mabar4 != '' 
begin
	insert into #tblResultB(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass from #LemitedResultOfRoad I join 
	#tblResultA R on I.SHAPE.STIntersects(R.Shape)=1 and I.name like N'%' + @Mabar4 + '%'

	insert into #tblResultIf_A_B_IsEmpty(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass  from #tblResultB I
	--select I.name, I.osm_id, I.Shape from OSM_ROADS I join 
	--#tblResultA R on 1=1
	--where  I.SHAPE.STIntersects(R.Shape.STBuffer(1))=1 and I.name like N'%' + @Mabar5 + '%'

	delete #tblResultA
end


if (select count(*) from #tblResultB) > 0 and @Mabar5 != '' 
begin
	insert into #tblResultA(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass from #LemitedResultOfRoad I join 
	#tblResultB R on I.SHAPE.STIntersects(R.Shape)=1 and I.name like N'%' + @Mabar5 + '%'


	insert into #tblResultIf_A_B_IsEmpty(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass  from #tblResultA I
	--select I.name, I.osm_id, I.Shape from OSM_ROADS I join 
	--#tblResultB R on 1=1
	--where  I.SHAPE.STIntersects(R.Shape.STBuffer(1))=1 and I.name like N'%' + @Mabar6 + '%'

	delete #tblResultB
end


if (select count(*) from #tblResultA) > 0 and @Mabar6 != '' 
begin
	insert into #tblResultB(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass from #LemitedResultOfRoad I join 
	#tblResultA R on I.SHAPE.STIntersects(R.Shape)=1 and I.name like N'%' + @Mabar6 + '%'

	insert into #tblResultIf_A_B_IsEmpty(name, osm_id, Shape, fclass)
	select I.name, I.osm_id, I.Shape, I.fclass  from #tblResultB I
	--select I.name, I.osm_id, I.Shape from OSM_ROADS I join 
	--#tblResultA R on 1=1
	--where  I.SHAPE.STIntersects(R.Shape.STBuffer(1))=1 and I.name like N'%' + @Mabar5 + '%'

	delete #tblResultA
end
------------------------------------------
------نتیجه

--select * from #tblResultA t order by 
--	case t.fclass
--        when 'trunk' then 1
--        when 'primary' then 2
--        when 'secondary' then 3
--		else 4
--    end, t.osm_id
--select * from #tblResultB t order by 
--	case t.fclass
--        when 'trunk' then 1
--        when 'primary' then 2
--        when 'secondary' then 3
--		else 4
--    end
--select * from #tblResultIf_A_B_IsEmpty t order by 
--	case t.fclass
--        when 'trunk' then 1
--        when 'primary' then 2
--        when 'secondary' then 3
--		else 4
--    end

if (select count(*) from #tblResultB) > 0
begin
	select top 1 @ResultShape = t.Shape from #tblResultB t 
	order by 
	case t.fclass
        when 'trunk' then 1
        when 'primary' then 2
        when 'secondary' then 3
		else 4
    end,
	t.osm_id 
end
else if (select count(*) from #tblResultA) > 0
begin
	select top 1  @ResultShape = t.Shape from #tblResultA t
	order by 
	case t.fclass
        when 'trunk' then 1
        when 'primary' then 2
        when 'secondary' then 3
		else 4
    end,
	t.osm_id 
end
else
begin
	select top 1  @ResultShape = t.Shape from #tblResultIf_A_B_IsEmpty t 
	order by
	--case t.fclass
 --       when 'trunk' then 1
 --       when 'primary' then 2
 --       when 'secondary' then 3
	--	else 4
 --   end,
	t.id desc 
end

set @strPoint =  @ResultShape.STBuffer(0.0001).STCentroid().ToString()
set @strPoint = REPLACE(@strPoint,N'POINT','')
set @strPoint = REPLACE(@strPoint,N'(','')
set @strPoint = REPLACE(@strPoint,N')','')

select ltrim(rtrim(@strPoint)) as Point

drop table #tblResultB
drop table #tblResultA
drop table #tblResultIf_A_B_IsEmpty
drop table #City
drop table #County

end


