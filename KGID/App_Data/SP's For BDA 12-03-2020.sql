USE [kgid_db]
GO
/****** Object:  StoredProcedure [dbo].[MOVE_DATA_STAGE_MAIN]    Script Date: 12-03-2020 10:07:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[KGID]
AS
BEGIN
INSERT INTO tbl_new_employee(em_employee_id,
em_employeename,em_employeecode,em_mobilenumber,em_dateofjoining,em_active,em_creation_datetime,em_updation_datetime,em_created_by,
em_updated_by,em_father_name,em_gender,em_date_of_birth,em_job_type,em_email,em_designation_id,em_place_of_posting,em_dept_code,
em_date_of_appointment,em_payscale_code,em_permanent_temporary,em_group
)

SELECT em_employee_id,em_employeename,em_employeecode,em_mobilenumber,em_dateofjoining,em_active,em_creation_datetime,em_updation_datetime,em_created_by,
em_updated_by,em_father_name,em_gender,em_date_of_birth,em_job_type,em_email,em_designation_id,em_place_of_posting,em_dept_code,
em_date_of_appointment,em_payscale_code,em_permanent_temporary,em_group
FROM tbl_new_employee

--DELETE FROM FLAT_STG
EXEC KGID
--COMMIT;
END
GO


