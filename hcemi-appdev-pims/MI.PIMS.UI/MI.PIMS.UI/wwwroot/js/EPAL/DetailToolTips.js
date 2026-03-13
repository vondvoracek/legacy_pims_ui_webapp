$(document).ready(function () {

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })


    //Header
    $('#txtPIMSID').attr('title', 'Unique alpha-numeric record identification for the PIMS database.');
    $('#txtProduct').attr('title', 'TBD');
    $('#txtProcedureCode').attr('title', 'The procedure code that is assigned to the record.');
    $('#txtPlan').attr('title', 'TBD');
    $('#txtFunding').attr('title', 'TBD');
    $('#txtEntity').attr('title', 'TBD');
    $('#txtState').attr('title', 'STATE_CD from EPAL_PROCS_APPL_TO_STATES_T table');
    $('#txtBusinessSegment').attr('title', 'TBD');


    //PA Code Summary
    $('#ddlCategory').attr('title', 'A measure set defined by CPT coding ranges that places each code into a specific set of healthcare categories.');
    $('#txtSubCategory').attr('title', 'A measure set defined by CPT coding ranges that places each code into a specific set of healthcare sub-categories.');
    $('#txtAHRQ1').attr('title', 'A measure set defined by AHRQ administrative data that places each code into a specific set of healthcare categories.')
    $('#txtHCEMICategory').attr('title', 'A measure set defined by CPT coding ranges that places each code into a specific set of healthcare categories used for reporting purposes.');
    $('#txtSource').attr('title', 'TBD');
    $('#txtCode').attr('title', 'The procedure code that is assigned to the record');
    $('#txtCodeDescription').attr('title', 'The short description for the procedure code associated with the prior auth record');
    $('#txtCodeType').attr('title', 'The type of code associated with the prior auth record');
    $('#txtDxCodeRequirments').attr('title', 'The determination if the code has diagnosis requirements. ');
    $('#divPriorAuth').attr('title', 'Denotes if the code requires prior auth');
    $('#divCodeStatus').attr('title', 'The status of the procedure code during the time of the prior auth');
    $('#divPAEffectiveDate').attr('title', 'The date the prior auth became effective');
    $('#divPAExpirationDate').attr('title', 'The date the prior auth expired');
    $('#txtDrugName').attr('title', 'The name of the drug associated with the prior auth record');
    $('#txtDrugReviewAtLaunch').attr('title', 'Indicates is the drug requires review at the time of the launch of the drug');
    $('#txtProgramManagedBy').attr('title', 'The department, vendor or team that manages the prior auth review');
    $('#txtInternalExternal').attr('title', 'Is the review managed internal or external to UHC?');
    $('#txtDelegatedUM').attr('title', 'TBD');
    $('#txtOverallPAStatus').attr('title', 'PRIOR_AUTH_IND from EPAL_PROCEDURES_T table');


    //PA Review Indicators
    $('#txtAutoApproval').attr('title', 'Denotes if the code is eligible for auto approval');
    $('#divAAEffectiveDate').attr('title', 'The date the auto approval  became effective');
    $('#divAAExpirationDate').attr('title', 'The date the auto approval  expired');
    $('#divMSPEffectiveDate').attr('title', 'The date the MSP became effective');
    $('#divMSPExpirationDate').attr('title', 'The date the MSP expired');
    $('#divSOSEffectiveDate').attr('title', 'The date the SOS became effective');
    $('#divSOSExpirationDate').attr('title', 'The date the SOS expired');
    $('#txtFurtherConsiderations').attr('title', 'TBD');
    $('#txtMedicareSpecialProcessing').attr('title', 'Denotes if the code is eligible for Medicare special processing');
    $('#txtTypeofSpecialProcessing').attr('title', 'Medicare Special Processing Type refers to the type of special processing the auth request has to go through. ');
    $('#txtLevelOfCareReview').attr('title', 'Denotes if the code is eligible for Level of Care Review');
    $('#txtMedicalNecessityReview').attr('title', 'Denotes if the code is eligible for Medical Necessity Review');
    $('#txtSiteofServiceApplies').attr('title', 'Denotes if the code is eligible for Site of Service Review');


});