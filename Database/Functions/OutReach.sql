CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_EVENT_DELETE"(p_eventid integer DEFAULT 0, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
begin
UPDATE "OR_APP"."OR_M_EVENT" 
SET  "IsDeleted"=NOT(COALESCE(p_isdeleted, "IsDeleted"))
WHERE "EventId"=p_eventid
RETURNING "EventId" into p_eventid;
return p_eventid;



end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_EVENT_GETALL"(p_pagenumber integer DEFAULT NULL::integer, p_pagesize integer DEFAULT NULL::integer, p_name text DEFAULT NULL::text, p_city text DEFAULT NULL::text)
 RETURNS TABLE("Id" integer, name character varying, description character varying, userid integer, "interestId" integer, publicevent boolean, capacity integer, "eventTime" date, streetaddress1 character varying, streetaddress2 character varying, city character varying, state character varying, zipcode character varying, country character varying, createdby character varying, createddate date, updateby character varying, updateddate date, "Active" boolean, "Deleted" boolean)
 LANGUAGE plpgsql
AS $function$


begin
	    return QUERY
select
	 "EventId" as "Id",
	"Name" as "name",
	"Description" as "description",
	"UserId" as "userid",
	"InterestId" as "interestid",
	"PublicEvent" as "publicevent",
	"Capacity" as "capacity",
	"EventTime" as "eventtime",
	"StreetAddress1" as "streetaddress1",
	"StreetAddress2" as "streetaddress2",
	"City" as "city",
	"State" as "state",
	"Zipcode" as "zipcode",
	"Country" as "country",
	"CreatedBy" as "createdby",
	"CreatedDate" as "createddate",
	"UpdateBy" as "updateby",
	"UpdatedDate" as "updateddate",
	"IsActive" as "isactive",
	"IsDeleted" as "isdeleted"
from
	"OR_APP"."OR_M_EVENT"
where
	(strpos("Name", p_name)>0)
	and 
(strpos("City", p_city)>0)
	
order by "EventId"
limit p_pagesize
   offset ((p_pagenumber-1) * p_pagesize);
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_EVENT_INSERT"(p_eventid integer DEFAULT 0, p_name text DEFAULT NULL::text, p_description text DEFAULT NULL::text, p_userid integer DEFAULT NULL::integer, p_interestid integer DEFAULT NULL::integer, p_publicevent boolean DEFAULT NULL::boolean, p_capacity integer DEFAULT NULL::integer, p_eventtime date DEFAULT NULL::date, p_streetaddress1 text DEFAULT NULL::text, p_streetaddress2 text DEFAULT NULL::text, p_city text DEFAULT NULL::text, p_state text DEFAULT NULL::text, p_zipcode text DEFAULT NULL::text, p_country text DEFAULT NULL::text, p_createdby text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());

begin
		if(exists(
select
	1
from
	"OR_APP"."OR_M_EVENT"
where
	"Name" = p_name))
		then return -1;
else
insert
	into
	"OR_APP"."OR_M_EVENT" (
	"Name",
	"Description" ,
	"UserId" ,
	"InterestId",
	"PublicEvent",
	"Capacity" ,
	"EventTime" ,
	"StreetAddress1",
	"StreetAddress2",
	"City" ,
	"State",
	"Zipcode",
	"Country" ,
		"CreatedBy" ,
		"CreatedDate" ,
		"UpdateBy" ,
		"UpdatedDate" ,
		"IsActive" ,
		"IsDeleted"
		)
values(p_name,
	p_description,
	p_userid,
	p_interestid,
	p_publicevent,
	p_capacity,
	p_eventtime,
	p_streetaddress1,
	p_streetaddress2,
	p_city,
	p_state,
	p_zipcode,
	p_country,
	p_createdby,
	CurrentDateUtc,
	p_updateby,
	CurrentDateUtc,
	p_isactive,
	p_isdeleted)
returning "EventId"
	into
		p_eventid;

return p_eventid;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_EVENT_SELECTBYID"(p_id integer DEFAULT NULL::integer)
 RETURNS TABLE(p_eventid integer, p_name character varying, p_description character varying, p_userid integer, p_interestid integer, p_publicevent character varying, p_capacity character varying, p_eventtime character varying, p_streetaddress1 character varying, p_streetaddress2 character varying, p_city character varying, p_state character varying, p_zipcode character varying, p_country character varying, p_createdby character varying, p_updateby character varying, p_isactive boolean, p_isdeleted boolean)
 LANGUAGE plpgsql
AS $function$
	BEGIN
	IF (exists(select * from outreachdb."OR_APP"."OR_M_EVENT" where outreachdb."OR_APP"."OR_M_EVENT"."EventId" = p_id)) THEN 
	return QUERY
	select "EventId" as p_id,
	"Name" as p_name,
	"Description" as p_description,
	"UserId" as p_userid,
	"InterestId" as p_interestid,
	"PublicEvent" as p_publicevent,
	"Capacity" as p_capacity,
	"EventTime" as p_eventtime,
	"StreetAddress1" as p_streetaddress1,
	"StreetAddress2" as p_streetaddress2,
	"City" as p_city,
	"State" as p_state,
	"Zipcode" as p_zipcode,
	"Country" as p_country,
	"CreatedBy" as p_createdby,
	"CreatedDate" as CurrentDateUtc,
	"UpdateBy" as p_updateby,
	"UpdatedDate" as CurrentDateUtc,
	"IsActive" as p_isactive,
	"IsDeleted" as p_isdeleted
	from outreachdb."OR_APP"."OR_M_EVENT"
	where outreachdb."OR_APP"."OR_M_EVENT"."EventId" = p_id;
else
	 RAISE EXCEPTION 'School % is not found!', p_id;
	end if;
	END;
$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_EVENT_UPDATE"(p_eventid integer DEFAULT 0, p_name text DEFAULT NULL::text, p_description text DEFAULT NULL::text, p_userid integer DEFAULT NULL::integer, p_interestid integer DEFAULT NULL::integer, p_publicevent boolean DEFAULT NULL::boolean, p_capacity integer DEFAULT NULL::integer, p_eventtime date DEFAULT NULL::date, p_streetaddress1 text DEFAULT NULL::text, p_streetaddress2 text DEFAULT NULL::text, p_city text DEFAULT NULL::text, p_state text DEFAULT NULL::text, p_zipcode text DEFAULT NULL::text, p_country text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());
	begin
		
		 IF COALESCE(p_eventid,0) <> 0 THEN
UPDATE "OR_APP"."OR_M_EVENT"
SET "Name"=COALESCE(p_name,"Name"), 
"Description"=COALESCE(p_description,"Description"),
"UserId"=COALESCE(p_userid,"UserId"), 
"InterestId"=COALESCE(p_interestid,"InterestId"),
"PublicEvent"=COALESCE(p_publicevent,"PublicEvent"), 
"Capacity"=COALESCE(p_capacity,"Capacity"),
"EventTime"=COALESCE(p_eventtime,"EventTime"), 
"StreetAddress1"=COALESCE(p_streetaddress1,"StreetAddress1"),
"StreetAddress2"=COALESCE(p_streetaddress2,"StreetAddress2"), 
"City"=COALESCE(p_city,"City"),
"State"=COALESCE(p_state,"State"),
"Zipcode"=COALESCE(p_zipcode,"Zipcode"), 
"Country"=COALESCE(p_country,"Country"),
"UpdateBy"=COALESCE(p_updateby,"UpdateBy"), 
"UpdatedDate"=CurrentDateUtc, 
"IsActive"=COALESCE(p_isactive,"IsActive"), 
"IsDeleted"=COALESCE(p_isdeleted,"IsDeleted")
WHERE "EventId"=p_eventid;



return 1;
else 
return 0;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_INTEREST_DELETE"(p_interestid integer DEFAULT 0, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
begin
UPDATE "OR_APP"."OR_M_INTEREST" 
SET  "IsDeleted"=NOT(COALESCE(p_isdeleted, "IsDeleted"))
WHERE "InterestId"=p_interestid 
RETURNING "InterestId" into p_interestid ;
return p_interestid ;



end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_INTEREST_GETALL"(p_pagenumber integer DEFAULT 1, p_pagesize integer DEFAULT 5, p_name text DEFAULT ''::text)
 RETURNS TABLE("InterestId" integer, "Name" character varying, "Category" character varying, "Icon" character varying, "Description" character varying, "CreatedBy" character varying, "CreatedDate" timestamp without time zone, "UpdateBy" character varying, "UpdatedDate" timestamp without time zone, "IsActive" boolean, "IsDeleted" boolean)
 LANGUAGE plpgsql
AS $function$


begin
	    return QUERY
select
	omi."InterestId", 
	omi."Name", 
	omi."Category", 
	omi."Icon", 
	omi."Description",
	omi."CreatedBy", 
	omi."CreatedDate", 
	omi."UpdateBy",
	omi."UpdatedDate",
	omi."IsActive", 
	omi."IsDeleted"
from
	"OR_APP"."OR_M_INTEREST" omi
where
	(strpos(omi."Name", p_name)>0)
order by
	omi."InterestId"
limit p_pagesize
   offset ((p_pagenumber-1) * p_pagesize);
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_INTEREST_INSERT"(p_interestid integer DEFAULT 0, p_name text DEFAULT NULL::text, p_category text DEFAULT NULL::text, p_icon text DEFAULT NULL::text, p_description text DEFAULT NULL::text, p_createdby text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());

begin
		if(exists(
select
	1
from
	"OR_APP"."OR_M_INTEREST"
where
	"Name" = p_name))
		then return -1;
else
insert
	into
		"OR_APP"."OR_M_INTEREST" (
	"Name" ,
	"Category" ,
	"Icon",
	"Description" ,
		"CreatedBy" ,
		"CreatedDate" ,
		"UpdateBy" ,
		"UpdatedDate" ,
		"IsActive" ,
		"IsDeleted"
		)
values(p_name,
	p_category,
	p_icon,
	p_description,
	p_createdby,
	CurrentDateUtc,
	p_updateby ,
	CurrentDateUtc,
	p_isactive ,
	p_isdeleted)
returning "InterestId"
	into
		p_interestid;

return p_interestid;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_INTEREST_SELECTBYID"(p_id integer DEFAULT 0)
 RETURNS TABLE("Id" integer, "IName" character varying, "ICategory" character varying, "IIcon" character varying, "IDescription" character varying, createdby character varying, createddate date, updateby character varying, updateddate date, "Active" boolean, "Deleted" boolean)
 LANGUAGE plpgsql
AS $function$


begin
	    if (exists(select * from outreachdb."OR_APP"."OR_M_INTEREST"
	    where outreachdb."OR_APP"."OR_M_INTEREST"."InterestId" = p_id)) then 
	return QUERY
select
	"InterestId" as "Id",
	"Name" as "IName",
	"Category" as "ICategory",
	"Icon" as "IIcon",
	"Description" as "IDescription",
	"CreatedBy" as "createdby",
	"CreatedDate" as "createddate",
	"UpdateBy" as "updateby",
	"UpdatedDate" as "updateddate",
	"IsActive" as "Active",
	"IsDeleted" as "Deleted"
from
	"OR_APP"."OR_M_INTEREST"
where
	outreachdb."OR_APP"."OR_M_INTEREST"."InterestId" = p_id;
else
	 raise exception 'Interest % is not found!',
p_id;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_INTEREST_UPDATE"(p_interestid integer DEFAULT 0, p_name text DEFAULT NULL::text, p_category text DEFAULT NULL::text, p_icon text DEFAULT NULL::text, p_description text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());
	begin
		
		 IF COALESCE(p_interestid,0) <> 0 THEN
UPDATE "OR_APP"."OR_M_INTEREST"
SET "Name"=COALESCE(p_name,"Name"), 
"Category"=COALESCE(p_category,"Category"),
"Icon"=COALESCE(p_icon,"Icon"),
"Description"=COALESCE(p_description,"Description"),
"UpdateBy"=COALESCE(p_updateby,"UpdateBy"), 
"UpdatedDate"=CurrentDateUtc, 
"IsActive"=COALESCE(p_name,"IsActive"),
"IsDeleted"=COALESCE(p_name,"IsDeleted")
WHERE "InterestId"=p_interestid;

return 1;
else 
return 0;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_SCHOOL_DELETE"(p_schoolid integer DEFAULT 0, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
begin
UPDATE "OR_APP"."OR_M_SCHOOL"  
SET  "IsDeleted"=NOT(COALESCE(p_isdeleted, "IsDeleted"))
WHERE "SchoolId"=p_schoolid
RETURNING "SchoolId" into p_schoolid;
return p_schoolid;



end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_SCHOOL_GETALL"(p_pagenumber integer DEFAULT 1, p_pagesize integer DEFAULT 5, p_name text DEFAULT ''::text, p_city text DEFAULT ''::text)
 RETURNS TABLE("SchoolId" integer, "Name" character varying, "StreetAddress1" character varying, "StreetAddress2" character varying, "City" character varying, "State" character varying, "Zipcode" character varying, "Country" character varying, "Latitude" double precision, "Longitude" double precision, "CreatedBy" character varying, "CreatedDate" timestamp without time zone, "UpdateBy" character varying, "UpdatedDate" timestamp without time zone, "IsActive" boolean, "IsDeleted" boolean)
 LANGUAGE plpgsql
AS $function$


begin
	    return QUERY
select
	oms."SchoolId",
	oms."Name",
	oms."StreetAddress1",
	oms."StreetAddress2",
	oms."City",
	oms."State",
	oms."Zipcode",
	oms."Country",
	oms."Latitude",
	oms."Longitude",
	oms."CreatedBy",
	oms."CreatedDate",
	oms."UpdateBy",
	oms."UpdatedDate",
	oms."IsActive",
	oms."IsDeleted"
from
	"OR_APP"."OR_M_SCHOOL" oms
where
	(strpos(oms."Name", p_name)>0)
	and 
(strpos(oms."City", p_city)>0)
order by
	oms."SchoolId"
limit p_pagesize
   offset ((p_pagenumber-1) * p_pagesize);
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_SCHOOL_INSERT"(p_schoolid integer DEFAULT 0, p_name text DEFAULT NULL::text, p_streetaddress1 text DEFAULT NULL::text, p_streetaddress2 text DEFAULT NULL::text, p_city text DEFAULT NULL::text, p_state text DEFAULT NULL::text, p_zipcode text DEFAULT NULL::text, p_country text DEFAULT NULL::text, p_latitude double precision DEFAULT NULL::double precision, p_longitude double precision DEFAULT NULL::double precision, p_createdby text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());

begin
		if(exists(
select
	1
from
	"OR_APP"."OR_M_SCHOOL"
where
	"Name" = p_name))
		then return -1;
else
insert
	into
		"OR_APP"."OR_M_SCHOOL" (
		"Name",
	"StreetAddress1" ,
	"StreetAddress2" ,
	"City" ,
	"State" ,
	"Zipcode" ,
	"Country" ,
	"Latitude" ,
	"Longitude",
		"CreatedBy" ,
		"CreatedDate" ,
		"UpdateBy" ,
		"UpdatedDate" ,
		"IsActive" ,
		"IsDeleted"
		)
values(p_name,
	p_streetaddress1,
	p_streetaddress2 ,
	p_city ,
	p_state ,
	p_zipcode ,
	p_country ,
	p_latitude ,
	p_longitude,
	p_createdby,
	CurrentDateUtc,
	p_updateby ,
	CurrentDateUtc,
	p_isactive ,
	p_isdeleted)
returning "SchoolId"
	into
		p_schoolid;

return p_schoolid;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_SCHOOL_SELECTBYID"(p_id integer DEFAULT 0)
 RETURNS TABLE("SchoolId" integer, "Name" character varying, "StreetAddress1" character varying, "StreetAddress2" character varying, "City" character varying, "State" character varying, "Zipcode" character varying, "Country" character varying, "Latitude" double precision, "Longitude" double precision, "CreatedBy" character varying, "CreatedDate" timestamp without time zone, "UpdateBy" character varying, "UpdatedDate" timestamp without time zone, "IsActive" boolean, "IsDeleted" boolean)
 LANGUAGE plpgsql
AS $function$
	begin
	if (exists(select 1 from	outreachdb."OR_APP"."OR_M_SCHOOL"  where outreachdb."OR_APP"."OR_M_SCHOOL"."SchoolId" = p_id)) then 
	return QUERY
	select
	oms."SchoolId",
	oms."Name",
	oms."StreetAddress1",
	oms."StreetAddress2",
	oms."City",
	oms."State",
	oms."Zipcode",
	oms."Country",
	oms."Latitude",
	oms."Longitude",
	oms."CreatedBy",
	oms."CreatedDate",
	oms."UpdateBy",
	oms."UpdatedDate",
	oms."IsActive",
	oms."IsDeleted"
from
	outreachdb."OR_APP"."OR_M_SCHOOL" oms
where
	oms."SchoolId" = p_id;
else
	 raise exception 'School % is not found!',
p_id;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_SCHOOL_UPDATE"(p_schoolid integer DEFAULT 0, p_name text DEFAULT NULL::text, p_streetaddress1 text DEFAULT NULL::text, p_streetaddress2 text DEFAULT NULL::text, p_city text DEFAULT NULL::text, p_state text DEFAULT NULL::text, p_zipcode text DEFAULT NULL::text, p_country text DEFAULT NULL::text, p_latitude double precision DEFAULT NULL::double precision, p_longitude double precision DEFAULT NULL::double precision, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());
	begin
		
		 IF COALESCE(p_schoolid,0) <> 0 THEN
update
	"OR_APP"."OR_M_SCHOOL"
set
	"Name" = COALESCE(p_name,"Name"),
	"StreetAddress1" = COALESCE(p_streetaddress1,"StreetAddress1"),
	"StreetAddress2" = COALESCE(p_streetaddress2,"StreetAddress2"),
	"City" = COALESCE(p_city,"City"),
	"State" = COALESCE(p_state,"State"),
	"Zipcode" = COALESCE(p_zipcode,"Zipcode"),
	"Country" = COALESCE(p_country,"Country"),
	"Latitude" = COALESCE(p_latitude,"Latitude"),
	"Longitude" = COALESCE(p_longitude,"Longitude"),
	"UpdateBy" = COALESCE(p_updateby,"UpdateBy"),
	"UpdatedDate" =CurrentDateUtc,
	"IsActive" = COALESCE(p_isactive,"IsActive"),
	"IsDeleted" = COALESCE(p_isdeleted,"IsDeleted")
where
	"SchoolId" = p_schoolid;
return 1;
else 
return 0;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_USER_DELETE"(p_userid integer DEFAULT 0, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
begin
UPDATE "OR_APP"."OR_M_USERDETAILS" 
SET  "IsDeleted"=NOT(COALESCE(p_isdeleted, "IsDeleted"))
WHERE "Userid"=p_userid
RETURNING "Userid" into p_userid;
return p_userid;



end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_USER_FORGOTPASSWORD"(p_useremail text DEFAULT NULL::text, OUT p_otp integer)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
	begin
		
insert
	into
	"OR_APP"."OR_T_OTP_TEST"
( "UserEmail",
	"OneTimeToken",
	"ValidTill",
	"ActiveFrom",
	"CreatedDate",
	"CreatedBy",
	"ModifiedDate",
	"ModifiedBy")
values(p_useremail,
floor(random()* (9999-1000 + 1) + 1000)::integer ,
current_timestamp + (5 || ' minutes')::interval ,
CURRENT_TIMESTAMP,
CURRENT_TIMESTAMP,
0,
CURRENT_TIMESTAMP,
0)
returning "OneTimeToken"
into
	p_otp;

end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_USER_GETALL"(p_pagenumber integer DEFAULT NULL::integer, p_pagesize integer DEFAULT NULL::integer, p_firstname text DEFAULT NULL::text, p_middlename text DEFAULT NULL::text, p_lastname text DEFAULT NULL::text, p_useremail text DEFAULT NULL::text)
 RETURNS TABLE("Id" integer, "Profile" character varying, "FName" character varying, "MName" character varying, "LName" character varying, "UGender" character varying, "UEmail" character varying, "UPassword" character varying, "UEducation" character varying, "UBio" character varying, "URole" integer, "USchoolid" integer, createdby character varying, createddate date, updateby character varying, updateddate date, "Active" boolean, "Deleted" boolean)
 LANGUAGE plpgsql
AS $function$


begin
	    return QUERY
select
	"Userid" as "Id",
	"ProfileImg" as "Profile",
	"Firstname" as "FName",
	"Middlename" as "MName",
	"Lastname" as "LName",
	"Gender" as "UGender",
	"UserEmail" as "UEmail",
	"UserPassword" as "UPassword",
	"Education" as "UEducation",
	"Bio" as "UBio",
	"Role" as "URole",
	"SchoolId" as "USchoolid",
	"CreatedBy" as "createdby",
	"CreatedDate" as "createddate",
	"UpdateBy" as "updateby",
	"UpdatedDate" as "updateddate",
	"IsActive" as "Active",
	"IsDeleted" as "Deleted"
FROM "OR_APP"."OR_M_USERDETAILS"
where
	(strpos("Firstname", p_firstname)>0)
	and 
(strpos("Middlename", p_middlename)>0)
and 
(strpos("Lastname", p_lastname)>0)
	and 
(strpos("UserEmail", p_useremail)>0)
	
	
order by "Userid"
limit p_pagesize
   offset ((p_pagenumber-1) * p_pagesize);
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_USER_INSERT"(p_userid integer DEFAULT 0, p_profileimg text DEFAULT NULL::text, p_firstname text DEFAULT NULL::text, p_middlename text DEFAULT NULL::text, p_lastname text DEFAULT NULL::text, p_gender text DEFAULT NULL::text, p_useremail text DEFAULT NULL::text, p_userpassword text DEFAULT NULL::text, p_education text DEFAULT NULL::text, p_bio text DEFAULT NULL::text, p_role integer DEFAULT NULL::integer, p_schoolid integer DEFAULT NULL::integer)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = now();

begin
		if(exists(
select
	1
from
	"OR_APP"."OR_M_USERDETAILS"
where
	"UserEmail" = lower(p_useremail) ))
		then return -1;
else
insert
	into
		"OR_APP"."OR_M_USERDETAILS" (
		"ProfileImg",
				"Firstname" ,
				"Middlename",
				"Lastname" ,
				"Gender" ,
				"UserEmail" ,
				"UserPassword",
				"Education" ,
				"Bio" ,
				"Role" ,
				"SchoolId",
				"CreatedBy" ,
				"CreatedDate" ,
				"UpdateBy" ,
				"UpdatedDate" ,
				"IsActive" ,
				"IsDeleted"
		)
values(p_profileimg,
	lower(p_firstname),
	lower(p_middlename) ,
	lower(p_lastname) ,
	lower(p_gender) ,
	lower(p_useremail) ,
	p_userpassword ,
	p_education ,
	p_bio ,
	p_role,
	p_schoolid,
	lower(p_useremail),
	CurrentDateUtc,
	lower(p_useremail) ,
	CurrentDateUtc,
	true,
	false
)
returning "Userid"
	into
		p_userid;

return p_userid;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_USER_OTP_VERIFY"(p_otp integer DEFAULT 0, p_useremail text DEFAULT NULL::text)
 RETURNS TABLE(otp integer)
 LANGUAGE plpgsql
AS $function$
--declare _currenttime timestamp without timezon;
begin

	return query
select
	case
		when ( "ActiveFrom" <= LOCALTIMESTAMP 
		and  "ValidTill" >= LOCALTIMESTAMP  )
		   then 1 --valid Otp for 5 minute 
		else 0 --invalid otp
	end as otp
from
	"OR_APP"."OR_T_OTP_TEST"
where
	"UserEmail" = p_useremail and  "OneTimeToken" = p_otp;

end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_USER_PASSWORD_UPDATE"(p_useremail text DEFAULT NULL::text, p_userpassword text DEFAULT NULL::text)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
begin
		
if(exists(
select
	1
from
	"OR_APP"."OR_M_USERDETAILS" 
where
	"UserEmail"  = p_useremail))
		then 
		UPDATE "OR_APP"."OR_M_USERDETAILS" 
SET "UserPassword"=COALESCE(p_userpassword,"UserPassword")
WHERE "UserEmail"=p_useremail;
return 1;
else 
return 0;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_USER_SELECTBYID"(p_id integer DEFAULT 0)
 RETURNS TABLE("Id" integer, "Profile" character varying, "FName" character varying, "MName" character varying, "LName" character varying, "UGender" character varying, "UEmail" character varying, "UPassword" character varying, "UEducation" character varying, "UBio" character varying, "URole" integer, "USchoolid" integer, createdby character varying, createddate timestamp without time zone, updateby character varying, updateddate timestamp without time zone, "Active" boolean, "Deleted" boolean)
 LANGUAGE plpgsql
AS $function$
	begin
	if (exists(
select
	*
from
	outreachdb."OR_APP"."OR_M_USERDETAILS"
where
	outreachdb."OR_APP"."OR_M_USERDETAILS"."Userid" = p_id)) then 
	return QUERY
	select
	"Userid" as "Id",
	"ProfileImg" as "Profile",
	"Firstname" as "FName",
	"Middlename" as "MName",
	"Lastname" as "LName",
	"Gender" as "UGender",
	"UserEmail" as "UEmail",
	"UserPassword" as "UPassword",
	"Education" as "UEducation",
	"Bio" as "UBio",
	"Role" as "URole",
	"SchoolId" as "USchoolid",
	"CreatedBy" as "createdby",
	"CreatedDate" as "createddate",
	"UpdateBy" as "updateby",
	"UpdatedDate" as "updateddate",
	"IsActive" as "Active",
	"IsDeleted" as "Deleted"
from
	outreachdb."OR_APP"."OR_M_USERDETAILS"
where
	outreachdb."OR_APP"."OR_M_USERDETAILS"."Userid" = p_id;
else
	 raise exception 'User % is not found!',
p_id;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_M_USER_UPDATE"(p_userid integer DEFAULT 0, p_profileimg text DEFAULT NULL::text, p_firstname text DEFAULT NULL::text, p_middlename text DEFAULT NULL::text, p_lastname text DEFAULT NULL::text, p_gender text DEFAULT NULL::text, p_useremail text DEFAULT NULL::text, p_userpassword text DEFAULT NULL::text, p_education text DEFAULT NULL::text, p_bio text DEFAULT NULL::text, p_role integer DEFAULT NULL::integer, p_schoolid integer DEFAULT NULL::integer, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$

declare CurrentDateUtc timestamp = now();

begin
		
		 if coalesce(p_userid, 0) <> 0 then
update
	"OR_APP"."OR_M_USERDETAILS"
set
	"ProfileImg" = coalesce(p_profileimg, "ProfileImg"),
	"Firstname" = coalesce(p_firstname, "Firstname"),
	"Middlename" = coalesce(p_middlename, "Middlename"),
	"Lastname" = coalesce(p_lastname, "Lastname"),
	"Gender" = coalesce(p_gender, "Gender"),
	"UserEmail" = coalesce(p_useremail, "UserEmail"),
	"UserPassword" = coalesce(p_userpassword, "UserPassword"),
	"Education" = coalesce(p_education, "Education"),
	"Bio" = coalesce(p_bio, "Bio"),
	"Role" = coalesce(p_role, "Role"),
	"SchoolId" = coalesce(p_schoolid, "SchoolId"),
	"UpdateBy" = coalesce(p_firstname, "UpdateBy"),
	"UpdatedDate" = CurrentDateUtc,
	"IsActive" = coalesce(p_isactive, "IsActive"),
	"IsDeleted" = coalesce(p_isdeleted, "IsDeleted")
where
	"Userid" = p_userid
	and "UserEmail" = p_useremail;

return 1;
else 
return 0;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_GROUPCHAT_DELETE"(p_groupchatid integer DEFAULT 0, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
begin
UPDATE "OR_APP"."OR_T_GROUPCHAT"
SET  "IsDeleted"=NOT(COALESCE(p_isdeleted, "IsDeleted"))
WHERE "GroupchatId"=p_tagsid
RETURNING "GroupchatId" into p_groupchatid;
return p_groupchatid;



end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_GROUPCHAT_INSERT"(p_groupchatid integer DEFAULT 0, p_userid integer DEFAULT 0, p_eventid integer DEFAULT 0, p_createdby text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());

begin
		IF COALESCE(p_groupchatid,0) <> 0 then 
		return -1;
else
insert
	into
		"OR_APP"."OR_T_GROUPCHAT" (
	
		"GroupchatId",
		"UserId" ,
		"EventId",
		"CreatedBy" ,
		"CreatedDate" ,
		"UpdateBy" ,
		"UpdatedDate" ,
		"IsActive" ,
		"IsDeleted"
		)
values(
	p_groupchatid,
	p_userid ,
	p_eventid ,
	p_createdby,
	CurrentDateUtc,
	p_updateby ,
	CurrentDateUtc,
	p_isactive ,
	p_isdeleted)
returning "GroupchatId"
	into
		p_groupchatid;

return p_groupchatid;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_GROUPCHAT_SELECTBYID"(p_id integer DEFAULT NULL::integer)
 RETURNS TABLE(p_groupchatid integer, p_userid bigint, p_eventid bigint, p_createdby character varying, p_createddate date, p_updateby character varying, p_updateddate date, p_isactive boolean, p_isdeleted boolean)
 LANGUAGE plpgsql
AS $function$
	begin
	if (exists(
select
	*
from
	outreachdb."OR_APP"."OR_T_GROUPCHAT"
where
	outreachdb."OR_APP"."OR_T_GROUPCHAT"."TagsId" = p_id)) then 
	return QUERY
	select
	"GroupchatId" as p_groupchatid,
		"UserId" as p_userid,
		"EventId" as p_eventid, 
		"CreatedBy" as p_createdby,
		"CreatedDate" as p_createddate,
		"UpdateBy" as p_updateby,
		"UpdatedDate" as p_updateddate,
		"IsActive" as p_isactive,
		"IsDeleted" as p_isdeleted
from
	outreachdb."OR_APP"."OR_T_GROUPCHAT"
where
	outreachdb."OR_APP"."OR_T_GROUPCHAT"."GroupchatId" = p_groupchatid;
else
	 raise exception 'Groupchat % is not found!',
p_id;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_GROUPCHAT_UPDATE"(p_groupchatid integer DEFAULT 0, p_userid integer DEFAULT 0, p_eventid integer DEFAULT 0, p_createdby text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());

begin
		
		 if coalesce(p_groupchatid, 0) <> 0 then
update
	"OR_APP"."OR_T_GROUPCHAT"
set
		"GroupchatId" = coalesce(p_groupchatid, "UserId"),
		"UserId" = coalesce(p_userid, "UserId"),
		"EventId" = coalesce(p_interestid, "EventId" ),
		"UpdateBy" = coalesce(p_updateby, "UpdateBy"),
		"UpdatedDate" = CurrentDateUtc,
		"IsActive" = coalesce(p_isactive, "IsActive" ),
		"IsDeleted" = coalesce(p_isdeleted, "IsDeleted")
where
	"GroupchatId" = p_groupchatid;

return 1;
else 
return 0;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_GROUPCHAT__GETALL"(p_pagenumber integer DEFAULT NULL::integer, p_pagesize integer DEFAULT NULL::integer)
 RETURNS TABLE(p_groupchatid integer, p_userid bigint, p_eventid bigint, p_createdby character varying, p_updateby character varying, p_isactive boolean, p_isdeleted boolean)
 LANGUAGE plpgsql
AS $function$
	BEGIN

		return QUERY
		select "GroupchatId" as p_groupchatid,
		"UserId" as p_userid,
		"EventId" as p_eventid, 
		"CreatedBy" as p_createdby,
		"CreatedDate" as p_createddate,
		"UpdateBy" as p_updateby,
		"UpdatedDate" as p_updateddate,
		"IsActive" as p_isactive,
		"IsDeleted" as p_isdeleted
		
		from outreachdb."OR_APP"."OR_T_GROUPCHAT"
    order by outreachdb."OR_APP"."OR_T_GROUPCHAT"."GroupchatId"
    limit p_pagesize
    OFFSET ((p_pagenumber-1) * p_pagesize);
		
	END;
$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_TAGS_DELETE"(p_tagsid integer DEFAULT 0, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
begin
UPDATE "OR_APP"."OR_T_TAGS"
SET  "IsDeleted"=NOT(COALESCE(p_isdeleted, "IsDeleted"))
WHERE "TagsId" =p_tagsid
RETURNING "TagsId" into p_tagsid;
return p_tagsid;



end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_TAGS_GETALL"(p_pagenumber integer DEFAULT NULL::integer, p_pagesize integer DEFAULT NULL::integer)
 RETURNS TABLE(p_tagsid integer, p_userid bigint, p_interestid bigint, p_createdby character varying, p_createddate date, p_updateby character varying, p_updateddate date, p_isactive boolean, p_isdeleted boolean)
 LANGUAGE plpgsql
AS $function$
	BEGIN

		return QUERY
		select "TagsId" as p_tagsid,
		"UserId" as p_userid,
		"InterestId" as p_interestid, 
		"CreatedBy" as p_createdby,
		"CreatedDate" as p_createddate,
		"UpdateBy" as p_updateby,
		"UpdatedDate" as p_updateddate,
		"IsActive" as p_isactive,
		"IsDeleted" as p_isdeleted
		
		from outreachdb."OR_APP"."OR_T_TAGS"
    order by outreachdb."OR_APP"."OR_T_TAGS"."TagsId"
    limit p_pagesize
    OFFSET ((p_pagenumber-1) * p_pagesize);
		
	END;
$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_TAGS_INSERT"(p_tagsid integer DEFAULT 0, p_userid integer DEFAULT 0, p_interestid integer DEFAULT 0, p_createdby text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());

begin
		--IF COALESCE(p_tagsid,0) <> 0 then 
		--return -1;
--else
insert
	into
		"OR_APP"."OR_T_TAGS" (
	
		"Userid",
		"InterestId" ,
		"CreatedBy" ,
		"CreatedDate" ,
		"UpdateBy" ,
		"UpdatedDate" ,
		"IsActive" ,
		"IsDeleted"
		)
values(
	p_userid ,
	p_interestid ,
	p_createdby,
	CurrentDateUtc,
	p_updateby ,
	CurrentDateUtc,
	p_isactive ,
	p_isdeleted)
returning "TagsId"
	into
		p_tagsid;

return p_tagsid;
--end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_TAGS_SELECTBYID"(p_id integer DEFAULT NULL::integer)
 RETURNS TABLE(p_tagsid integer, p_userid bigint, p_interestid bigint, p_createdby character varying, p_createddate date, p_updateby character varying, p_updateddate date, p_isactive boolean, p_isdeleted boolean)
 LANGUAGE plpgsql
AS $function$
	begin
	if (exists(
select
	*
from
	outreachdb."OR_APP"."OR_T_TAGS"
where
	outreachdb."OR_APP"."OR_T_TAGS"."TagsId" = p_id)) then 
	return QUERY
	select
	"TagsId" as p_tagsid,
		"UserId" as p_userid,
		"InterestId" as p_interestid, 
		"CreatedBy" as p_createdby,
		"CreatedDate" as p_createddate,
		"UpdateBy" as p_updateby,
		"UpdatedDate" as p_updateddate,
		"IsActive" as p_isactive,
		"IsDeleted" as p_isdeleted
from
	outreachdb."OR_APP"."OR_T_TAGS"
where
	outreachdb."OR_APP"."OR_T_TAGS"."TagsId" = p_id;
else
	 raise exception 'Tags % is not found!',
p_id;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_TAGS_UPDATE"(p_tagsid integer DEFAULT 0, p_userid integer DEFAULT 0, p_interestid integer DEFAULT 0, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());

begin
		
		 if coalesce(p_tagsid, 0) <> 0 then
update
	"OR_APP"."OR_T_TAGS"
set
	
		"UserId" = coalesce(p_userid, "UserId"),
		"InterestId" = coalesce(p_interestid, "InterestId" ),
		"UpdateBy" = coalesce(p_updateby, "UpdateBy"),
		"UpdatedDate" = CurrentDateUtc,
		"IsActive" = coalesce(p_isactive, "IsActive" ),
		"IsDeleted" = coalesce(p_isdeleted, "IsDeleted")
where
	"TagsId" = p_tagsid;

return 1;
else 
return 0;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_USEREVENT_DELETE"(p_usereventid integer DEFAULT 0, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
begin
UPDATE "OR_APP"."OR_T_USEREVENT" 
SET  "IsDeleted"=NOT(COALESCE(p_isdeleted, "IsDeleted"))
WHERE "UserEventId"=p_usereventid
RETURNING "UserEventId" into p_usereventid;
return p_usereventid;



end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_USEREVENT_GETALL"(p_pagenumber integer DEFAULT NULL::integer, p_pagesize integer DEFAULT NULL::integer)
 RETURNS TABLE(p_usereventid integer, p_userid bigint, p_eventid bigint, p_status boolean, p_createdby character varying, p_createddate date, p_updateby character varying, p_updateddate date, p_isactive boolean, p_isdeleted boolean)
 LANGUAGE plpgsql
AS $function$
	BEGIN

		return QUERY
		select "UserEventId" as p_usereventid,
		"Userid" as p_userid,
		"EventId" as p_eventid, 
		"Status" as p_status,
		"CreatedBy" as p_createdby,
		"CreatedDate" as p_createddate,
		"UpdateBy" as p_updateby,
		"UpdatedDate" as p_updateddate,
		"IsActive" as p_isactive,
		"IsDeleted" as p_isdeleted
	
		from outreachdb."OR_APP"."OR_T_USEREVENT" 
    order by outreachdb."OR_APP"."OR_T_USEREVENT"."UserEventId"
    limit p_pagesize
    OFFSET ((p_pagenumber-1) * p_pagesize);
		
	END;
$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_USEREVENT_INSERT"(p_usereventid integer DEFAULT 0, p_userid integer DEFAULT 0, p_eventid integer DEFAULT 0, p_status boolean DEFAULT true, p_createdby text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());

begin
		--IF COALESCE(p_tagsid,0) <> 0 then 
		--return -1;
--else
insert
	into
		"OR_APP"."OR_T_USEREVENT" (
	
		"Userid",
		"EventId" ,
		"Status",
		"CreatedBy" ,
		"CreatedDate" ,
		"UpdateBy" ,
		"UpdatedDate" ,
		"IsActive" ,
		"IsDeleted"
		)
values(
	p_userid ,
	p_eventid ,
	p_status,
	p_createdby,
	CurrentDateUtc,
	p_updateby ,
	CurrentDateUtc,
	p_isactive ,
	p_isdeleted)
returning "UserEventId"
	into
		p_usereventid;

return p_usereventid;
--end if;
end;


$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."FN_OR_T_USEREVENT_UPDATE"(p_usereventid integer DEFAULT 0, p_userid integer DEFAULT 0, p_eventid integer DEFAULT 0, p_status boolean DEFAULT true, p_createdby text DEFAULT NULL::text, p_updateby text DEFAULT NULL::text, p_isactive boolean DEFAULT NULL::boolean, p_isdeleted boolean DEFAULT NULL::boolean)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
declare CurrentDateUtc timestamp = timezone('UTC',
now());

begin
		
		 if coalesce(p_usereventid, 0) <> 0 then
update
	"OR_APP"."OR_T_USEREVENT"
set
	
		"Userid" = coalesce(p_userid, "Userid"),
		"EventId" = coalesce(p_eventid, "EventId" ),
		"Status" = coalesce(p_status, "Status" ),
		"CreatedBy" = coalesce(p_createdby, "CreatedBy"),
		"CreatedDate" = CurrentDateUtc,
		"UpdateBy" = coalesce(p_updateby, "UpdateBy"),
		"UpdatedDate" = CurrentDateUtc,		
		"IsActive" = coalesce(p_isactive, "IsActive" ),
		"IsDeleted" = coalesce(p_isdeleted, "IsDeleted")
where
	"UserEventId" = p_usereventid;

return 1;
else 
return 0;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP"."OR_T_USEREVENT_SELECTBYID"(p_id integer DEFAULT NULL::integer)
 RETURNS TABLE(p_usereventid integer, p_userid bigint, p_eventid bigint, p_status boolean, p_createdby character varying, p_createddate date, p_updateby character varying, p_updateddate date, p_isactive boolean, p_isdeleted boolean)
 LANGUAGE plpgsql
AS $function$
	begin
	if (exists(
select
	*
from
	outreachdb."OR_APP"."OR_T_USEREVENT"
where
	outreachdb."OR_APP"."OR_T_USEREVENT"."UserEventId" = p_id)) then 
	return QUERY
	select
	"UserEventId" as p_tagsid,
		"Userid" as p_userid,
		"EventId" as p_eventid, 
		"Status" as p_status,
		"CreatedBy" as p_createdby,
		"CreatedDate" as p_createddate,
		"UpdateBy" as p_updateby,
		"UpdatedDate" as p_updateddate,
		"IsActive" as p_isactive,
		"IsDeleted" as p_isdeleted
from
	outreachdb."OR_APP"."OR_T_USEREVENT"
where
	outreachdb."OR_APP"."OR_T_USEREVENT"."UserEventId" = p_id;
else
	 raise exception 'UserEvent % is not found!',
p_id;
end if;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP".fn_auth_get_user_by_email(p_username text DEFAULT NULL::text, p_password text DEFAULT NULL::text)
 RETURNS TABLE("UserId" integer, "UserName" character varying, "UserCode" integer, "FirstName" character varying, "MiddleName" character varying, "LastName" character varying, "Email" character varying)
 LANGUAGE plpgsql
AS $function$
	begin	
return query
select
	"Userid" as "UserId" ,
	"UserEmail"  as "UserName",
	"Userid" as "UserCode",
	"Firstname"as"FirstName",
	"Middlename"as"MiddleName",
	"Lastname"as"LastName",
	"UserEmail" as "Email" 
from
	"OR_APP"."OR_M_USERDETAILS"
where "UserEmail" = p_username and "UserPassword" = p_password ;
end;

$function$
;

CREATE OR REPLACE FUNCTION "OR_APP".fn_auth_save_user_auth_session(p_sessionid character, p_usercode integer, p_useremail text, p_onetimetoken text, p_validtill timestamp without time zone)
 RETURNS integer
 LANGUAGE plpgsql
AS $function$

	BEGIN

		INSERT INTO "OR_APP"."OR_T_UserAuthSession"
("SessionId", "UserCode", "UserEmail", "OneTimeToken", "ValidTill", "ActiveFrom", "CreatedDate", "CreatedBy", "ModifiedDate", "ModifiedBy")
VALUES(p_sessionid, p_usercode, p_useremail, p_onetimetoken, p_validtill, LOCALTIMESTAMP, LOCALTIMESTAMP, 0, LOCALTIMESTAMP, 0);
return 1;
		
	END;
$function$
;
