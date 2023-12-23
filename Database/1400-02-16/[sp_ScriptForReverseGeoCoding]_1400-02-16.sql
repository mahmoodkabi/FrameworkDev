USE [OSM]
GO
/****** Object:  StoredProcedure [dbo].[sp_ScriptForReverseGeoCoding]    Script Date: 5/6/2021 6:15:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[sp_ScriptForReverseGeoCoding]
as
begin

------------------------------ اضافه كردن كليد به جدول IranNode
--ALTER TABLE dbo.IranNode
--   ADD IranNodeId INT IDENTITY
--       CONSTRAINT PK_IranNode PRIMARY KEY CLUSTERED

-------------------اضافه كردن فيلدهاي شهرستان و شهر به جدول ----------------------------------

	--Alter Table IranNode
	--Add 
	--CountyOsmId nvarchar(10),
	--CityOsmId nvarchar(10)

	--Alter Table OSM_Roads
	--Add 
	--CountyOsmId nvarchar(10),
	--CityOsmId nvarchar(10

	--Alter Table OSM_PLACES
	--Add 
	--CountyOsmId nvarchar(10),
	--CityOsmId nvarchar(10)

-----**************************************************************************------
-----در جدول هاي زير مشخص مي كنيم هر سطر متعلق به كدام شهرستان و شهر است
----------------------- IranNode شهرستان----------------------------
--select *  
update IranNode set [CountyOsmId] = A.osm_id
from
OSM_ROADS road join
(select * from OSM_PLACES_A place where place.fclass = 'county') A on A.SHAPE.STIntersects(road.SHAPE)=1 
join IranNode I on I.FromNode = road.osm_id



--------------------------- IranNode شهر --------------------------------------------
--select  *  
update IranNode set [CityOsmId] = A.osm_id
from
OSM_ROADS road join
(select * from OSM_PLACES_A place where place.fclass in('city', 'town', 'island')) A on A.SHAPE.STIntersects(road.SHAPE)=1 
join IranNode I on I.FromNode = road.osm_id



-----------------------OSM_ROADS شهرستان----------------------------

--select *  
update OSM_ROADS set [CountyOsmId] = A.osm_id
from
OSM_ROADS road join
(select * from OSM_PLACES_A place where place.fclass = 'county') A on A.SHAPE.STIntersects(road.SHAPE)=1 


---------------------------OSM_ROADS شهر--------------------------------------------
--select  *  
update OSM_ROADS set [CityOsmId] = A.osm_id
from
OSM_ROADS road join
(select * from OSM_PLACES_A place where place.fclass in('city', 'town', 'island')) A on A.SHAPE.STIntersects(road.SHAPE)=1 








---------------------------OSM_Place شهرستان--------------------------------------------
--select *  
update OSM_PLACES set [CountyOsmId] = A.osm_id
from
OSM_PLACES road join
(select * from OSM_PLACES_A place where place.fclass = 'county') A on A.SHAPE.STIntersects(road.SHAPE)=1 


---------------------------OSM_ROADS شهر--------------------------------------------
--select  *  
update OSM_PLACES set [CityOsmId] = A.osm_id
from
OSM_PLACES road join
(select * from OSM_PLACES_A place where place.fclass in('city', 'town', 'island')) A on A.SHAPE.STIntersects(road.SHAPE)=1 


end


