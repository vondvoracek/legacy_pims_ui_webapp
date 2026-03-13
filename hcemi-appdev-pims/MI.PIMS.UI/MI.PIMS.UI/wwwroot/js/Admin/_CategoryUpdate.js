
var _categoryUpdateController = {
    init: function () {

        MIApp.Common.ApiEPRepository.set('category-update-urls', 'CategoryUpdateUrls');

        $('#addCategoryModal,#editCategoryModal,#updateByAltCatModal,#updateByProcCodeModal').on('shown.bs.modal', function () {
            $(document).off('focusin.modal');
        });
        $("#btnSearch").unbind().on('click', function (e) {
            e.preventDefault();
            window.kendo.ui.progress($("#grid_category_update"), true);
            _categoryUpdateController.grid.refresh();
        });
        $('#btnReset').bind('click', function () {
            _categoryUpdateController.grid.clear();
        });

        // #region CategoryEdit
        $('#btnValidateEdit').unbind().on('click', function (ev) {
            ev.preventDefault();
            _categoryUpdateController.edit.validate();
        });
        $('#btnResetEdit').unbind().on('click', function (ev) {
            ev.preventDefault();
            _categoryUpdateController.edit.reset();
        });
        // #endregion CategoryEdit

        // #region CategoryUBAC
        $('#updateByAltCatModal').on('hide.bs.modal', function () {
            $('#btnResetUBAC').trigger('click');
        });
        $('#btnValidateUBAC').unbind().on('click', function (ev) {
            ev.preventDefault();
            _categoryUpdateController.updateByAltCat.validate();
        });
        $('#btnResetUBPC').bind('click', function () {
            _categoryUpdateController.updateByProcCode.reset();
        });        
        // #endregion CategoryUBAC

        // #region CategoryAdd
        $('#btnValidateAdd').unbind().on('click', function (ev) {
            ev.preventDefault();
            _categoryUpdateController.add.validate();
        });
        $('#addCategoryModal').on('hide.bs.modal', function () {
            $('#btnResetAdd').trigger('click');
        });
        // #endregion

        // #region CategoryUBPC
        $('#updateByProcCodeModal').on('hide.bs.modal', function () {
            $('#btnResetUBPC').trigger('click');
        });

        $('#formUBPC,#formAddCategory,#formUBAC').bind("keypress", function (e) {
            if (e.keyCode == 13) {
                return false;
            }
        });
        $('#btnValidateUBPC').unbind().on('click', function (ev) {
            ev.preventDefault();
            _categoryUpdateController.updateByProcCode.validate();
        });
        $('#ProcedureCode_UBPC').bind("keypress", function (e) {
            if (e.keyCode == 13) {
                _categoryUpdateController.updateByProcCode.addToList($('#ProcedureCode_UBPC').val());
                $('#ProcedureCode_UBPC').val('');
            }
        });
        // #endregion CategoryUBPC

    },
    add: {
        validate: function () {
            if ($('#NewProcedureCode').data("kendoDropDownList").value() == '') {
                MICore.Notification.warning('Invalid Procedure Code', 'Please enter Procedure code first!');
                return;
            }

            if ($("#NewAlternateCategory").data("kendoDropDownList").value() == '') {
                MICore.Notification.warning('Invalid Alternate category', 'Please select Alternate category!');
                return;
            }

            var categoryUpdateParam = {
                P_PROC_CD: $('#NewProcedureCode').data("kendoDropDownList").value(),
                P_DRUG_NM: $('#NewSpecialty').data("kendoDropDownList").value(),
                P_ALTERNATE_CATEGORY: $("#NewAlternateCategory").data("kendoDropDownList").value(),
                P_ALTERNATE_SUB_CATEGORY: $("#NewAlternateSubCategory").data("kendoDropDownList").value()
            };

            MICore.Notification.whileSaving('Saving changes', function () {
                _categoryUpdateController.updateByProcCode.validateDupAltCountNUpdate(categoryUpdateParam, insert);
            });

            function insert() {

                var categoryUpdateParam = {
                    P_PROC_CD: $('#NewProcedureCode').data("kendoDropDownList").value(),
                    P_DRUG_NM: $('#NewSpecialty').data("kendoDropDownList").value(),
                    P_ALTERNATE_CATEGORY: $("#NewAlternateCategory").data("kendoDropDownList").value(),
                    P_ALTERNATE_SUB_CATEGORY: $("#NewAlternateSubCategory").data("kendoDropDownList").value(),
                    P_LST_UPDT_BY: MS_ID
                };

                MICore.Api.post(MIApp.Common.ApiEPRepository.get('admin-insert-alt-cat-url', 'CategoryUpdateUrls'), categoryUpdateParam, function (updateDto) {
                    if (updateDto.StatusType == 'Error')
                        MICore.Notification.error('Error while adding Alternate Category', updateDto.Message);
                    else if (updateDto.StatusID == 2) {
                        MICore.Notification.warning('Record partially updated!', updateDto.Message);
                    }
                    else
                    {
                        MICore.Notification.success('Saved', updateDto.Message, function () {
                            $('#addCategoryModal').modal('hide');

                            _categoryUpdateController.grid.paramSelectionCheck();
                        });
                    }
                });
            }
        }
    },
    edit: {
        popup: function (PROC_CD, ALTERNATE_CATEGORY, ALTERNATE_SUB_CATEGORY) {
            $("#ProcedureCode_Edit").val(PROC_CD);
            $("#AlternateCategory_Edit").data("kendoDropDownList").value(ALTERNATE_CATEGORY);
            $('#AlternateCategory_Edit_Old').val(ALTERNATE_CATEGORY == 'null' ? '' : ALTERNATE_CATEGORY);

            $("#AlternateSubCategory_Edit").data("kendoDropDownList").value(ALTERNATE_SUB_CATEGORY);
            $('#AlternateSubCategory_Edit_Old').val(ALTERNATE_SUB_CATEGORY == 'null' ? '': ALTERNATE_SUB_CATEGORY);
            
            $('#editCategoryModal').modal('show');
        },
        deleteVisible: function (dataItem) {
            return dataItem.EDITABLE_IND == 'Y';
        },
        getRecordsImpacted: function (categoryUpdateParam, callback, callbackType) {
            var ri = null;
            MICore.Api.post(MIApp.Common.ApiEPRepository.get('admin-get-records-impacted-url', 'CategoryUpdateUrls'), categoryUpdateParam, function (recordsImpacted) {
                ri = recordsImpacted
                if (ri > 0) {
                    MICore.Notification.question(
                        'EPAL Records Impacted ', ri + ' number of EPAL records currently leverage this alternate category. Do you want to proceed with this ' + callbackType +'?', 'Yes', 'Cancel Edit',
                        callback, null
                    );
                } else {
                    callback();
                }
            }, null);
        },
        validate: function () {

            if ($("#ProcedureCode_Edit").val() == '') {
                MICore.Notification.warning('Invalid Procedure Code', 'Please re-select record, Procedure code is not selected!');
                return;
            }

            if ($("#AlternateCategory_Edit").data("kendoDropDownList").value() == '') {
                MICore.Notification.warning('Invalid Alternate Category', 'Please select Alternate Category!');
                return;
            }

            var categoryUpdateParam = {
                P_PROC_CD: $("#ProcedureCode_Edit").val().toUpperCase(),
                P_DRUG_NM: null,
                P_ALTERNATE_CATEGORY: $("#AlternateCategory_Edit_Old").val(),
                P_ALTERNATE_SUB_CATEGORY: $("#AlternateSubCategory_Edit_Old").val()
            }
            
            MICore.Notification.whileSaving('Saving changes', function () {
                // get epal records using this category
                _categoryUpdateController.edit.getRecordsImpacted(categoryUpdateParam, update, 'edit'); 
            });

            function update() {

                CategoryUpdate_Edit_Dto = {
                    P_PROC_CD: $('#ProcedureCode_Edit').val().toUpperCase(),
                    P_DRUG_NM: null,
                    P_ALTERNATE_CATEGORY_OLD: $('#AlternateCategory_Edit_Old').val(),
                    P_ALTERNATE_SUB_CATEGORY_OLD: $('#AlternateSubCategory_Edit_Old').val(),
                    P_ALTERNATE_CATEGORY_NEW: $("#AlternateCategory_Edit").data("kendoDropDownList").value(),
                    P_ALTERNATE_SUB_CATEGORY_NEW: $("#AlternateSubCategory_Edit").data("kendoDropDownList").value(),
                    P_LST_UPDT_BY: MS_ID,
                    P_LST_UPDT_DT: null
                }

                MICore.Api.post(MIApp.Common.ApiEPRepository.get('admin-update-cat-url', 'CategoryUpdateUrls'), CategoryUpdate_Edit_Dto, function (updateDto) {
                    if (updateDto.StatusType == 'Error')
                        MICore.Notification.error('Error while updating Alternate Category', updateDto.Message);
                    else {
                        MICore.Notification.success('Saved', updateDto.Message, function () {                            
                            $('#editCategoryModal').modal('hide');
                            _categoryUpdateController.grid.paramSelectionCheck();
                        });
                    }
                });
            }
        },
        reset: function () {
            $('#AlternateCategory_Edit').data("kendoDropDownList").value($('#AlternateCategory_Edit_Old').val());
            $('#AlternateSubCategory_Edit').data("kendoDropDownList").value($('#AlternateSubCategory_Edit_Old').val());
        }
    },
    delete: {
        validate: function (PROC_CD, ALTERNATE_CATEGORY, ALTERNATE_SUB_CATEGORY) {

            MICore.Notification.question('Delete confirmation!', 'Are you sure, you want to delete this record?', 'Yes', 'No', function () {

                var categoryUpdateParam = {
                    P_PROC_CD: PROC_CD,
                    P_DRUG_NM: null,
                    P_ALTERNATE_CATEGORY: ALTERNATE_CATEGORY == 'null' ? '' : ALTERNATE_CATEGORY,
                    P_ALTERNATE_SUB_CATEGORY: ALTERNATE_SUB_CATEGORY == 'null' ? '' : ALTERNATE_SUB_CATEGORY
                }                

                MICore.Notification.whileSaving('Deleting record', function () {
                    _categoryUpdateController.edit.getRecordsImpacted(categoryUpdateParam, deleteR, 'delete');
                });
            })

            function deleteR() {
                var CategoryUpdateParam = {
                    P_PROC_CD: PROC_CD,
                    P_ALTERNATE_CATEGORY: ALTERNATE_CATEGORY,
                    P_ALTERNATE_SUB_CATEGORY: ALTERNATE_SUB_CATEGORY == 'null' ? null : ALTERNATE_SUB_CATEGORY
                };

                MICore.Api.post(MIApp.Common.ApiEPRepository.get('admin-delete-alt-cat-url', 'CategoryUpdateUrls'), CategoryUpdateParam, function (updateDto) {
                    if (updateDto.StatusType == 'Error')
                        MICore.Notification.error('Error while updating Alternate Category', updateDto.Message);
                    else {
                        MICore.Notification.success('Deleted', updateDto.Message, function () {
                            _categoryUpdateController.grid.refresh();
                        });
                    }
                });
            }
        }
    },
    updateByProcCode: {
        reset: function () {
            $('#lsvProcedureCodesUBPC').data("kendoListView").dataSource.data([]);
        },
        addToList: function (proc_cds) {
            proc_cds = (proc_cds.replace(/\r\n/g, ",").replace(/\r/g, ",").replace(/\n/g, ",").replace(/ /g, ","));

            var $lsvProcCode_ubpc = $("#lsvProcedureCodesUBPC").data("kendoListView");
            var lsvProcCodeItems = $lsvProcCode_ubpc.dataSource;
            proc_cds.split(',').forEach(function (pc) {
                //debugger;
                if ($.trim(pc) != '') {
                    var existingItem = $.grep(lsvProcCodeItems.view(), function (m) { return m.PROC_CD == pc; })
                    if (existingItem == '' || existingItem == null)
                        $lsvProcCode_ubpc.dataSource.insert(0, { PROC_CD: pc.toUpperCase() });
                }
            });

        },
        getProcCDs: function () {
            var $lsvProcCode_ubpc = $("#lsvProcedureCodesUBPC").data("kendoListView");
            var proc_cds = [];
            $lsvProcCode_ubpc.dataSource.view().forEach(function (pc) {
                proc_cds.push(pc.PROC_CD);
            });
            return proc_cds;
        },
        validate: function () {

            var $lsvProcCode_ubpc = $("#lsvProcedureCodesUBPC").data("kendoListView");

            if ($lsvProcCode_ubpc.dataSource.view().length == 0) {
                MICore.Notification.warning('Invalid Procedure Codes', 'Please enter Procedure codes first!');
                return;
            }

            if ($("#AlternateCategory_UBPC").data("kendoDropDownList").value() == '') {
                MICore.Notification.warning('Invalid Alternate category', 'Please select Alternate category!');
                return;
            }

            var proc_cds = _categoryUpdateController.updateByProcCode.getProcCDs();

            var categoryUpdateParam = {
                P_PROC_CD: proc_cds.join(','),
                P_DRUG_NM: null,
                P_ALTERNATE_CATEGORY: $("#AlternateCategory_UBPC").data("kendoDropDownList").value(),
                P_ALTERNATE_SUB_CATEGORY: $("#AlternateSubCategory_UBPC").data("kendoDropDownList").value()
            };

            MICore.Notification.whileSaving('Saving changes', function () {
                insert();
            });

            function insert() {
                var proc_cds = _categoryUpdateController.updateByProcCode.getProcCDs();
                var categoryInsertByProcCDParams = [];
                proc_cds.forEach(function (pc) {
                    if ($.trim(pc) != null && $.trim(pc) != '') {
                        categoryInsertByProcCDParams.push(
                            {
                                P_PROC_CD: pc.toUpperCase(),
                                P_ALTERNATE_CATEGORY: $("#AlternateCategory_UBPC").data("kendoDropDownList").value(),
                                P_ALTERNATE_SUB_CATEGORY: $("#AlternateSubCategory_UBPC").data("kendoDropDownList").value(),
                                P_LST_UPDT_BY: MS_ID
                            }
                        );
                    }
                });

                var token = $('meta[name="request-verification-token"]')[0].content;

                $.ajax({
                    type: "POST",
                    url: MIApp.Common.ApiEPRepository.get('admin-insert-cat-by-proccd-url', 'CategoryUpdateUrls'),
                    async: true,
                    cache: false,
                    headers: {
                        'Accept': 'application/json',
                        'RequestVerificationToken': token
                    },
                    data: JSON.stringify(categoryInsertByProcCDParams),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (insertRetDto) {
                        if (insertRetDto.StatusType == 'Error')
                            MICore.Notification.error('Error while updating Alternate Category', updateDto.Message);
                        else {

                            var successStatus = {
                                title: '',
                                type: '',
                                message: 'Please see below the status of the Save progress.',
                                insRecs: '',
                                dupRecs: '',
                                errRecs: '',
                                callback: null
                            };

                            if (insertRetDto.ErrorRecords != null) {
                                successStatus.errRecs += insertRetDto.ErrorRecords.map(function (er) {
                                    return er.P_PROC_CD;
                                }).join(', ');

                                successStatus.type = 'warning';
                            }

                            if (insertRetDto.InsertedRecords != null) {
                                successStatus.insRecs += insertRetDto.InsertedRecords.map(function (ins) {
                                    return ins.P_PROC_CD;
                                }).join(', ');

                                successStatus.type = 'success';
                            }

                            if (insertRetDto.DupRecords != null) {
                                successStatus.dupRecs += insertRetDto.DupRecords.map(function (dp) {
                                    return dp.P_PROC_CD;
                                }).join(', ');

                                successStatus.type = 'warning';
                            }

                            var htmlTemplate =
                                "<div class='mb-3 font-weight-bold'>{{message}}</div> \
                                <table class='table table-bordered'> \
                                    <thead> \
                                        <tr> \
                                            <th scope='col'>Inserted Records</th> \
                                            <th scope='col'>Duplicate Records</th> \
                                            <th scope='col'>Invalid/Erroneous Records</th> \
                                         </tr> \
                                    </thead> \
                                    <tbody> \
                                        {{$trs}} \
                                    </tbody> \
                                </table>";

                            var rowTemplate =
                                "<tr> \
                                    <td>{{insTd}}</td> \
                                    <td>{{dupTd}}</td> \
                                    <td>{{errTd}}</td> \
                                </tr>";                            

                            rowTemplate = rowTemplate.replace('{{insTd}}', successStatus.insRecs);
                            rowTemplate = rowTemplate.replace('{{dupTd}}', successStatus.dupRecs);
                            rowTemplate = rowTemplate.replace('{{errTd}}', successStatus.errRecs);                            

                            htmlTemplate = htmlTemplate.replace('{{$trs}}', rowTemplate).replace('{{message}}', successStatus.message);

                            MICore.Notification.success(successStatus.title, htmlTemplate, function () {
                                $('#updateByProcCodeModal').modal('hide'); _categoryUpdateController.grid.paramSelectionCheck();
                            });
                        }
                    },
                    error: function (e) {
                        console.error(e);
                    }
                });
            }
        },
        validateDupAltCountNUpdate: function (categoryUpdateParam, insert) {
            MICore.Api.post(MIApp.Common.ApiEPRepository.get('admin-get-dup-alt-cat-count-url', 'CategoryUpdateUrls'), categoryUpdateParam, function (dc) {
                dupCount = dc;
                if (dupCount > 0) {
                    MICore.Notification.warning('Duplicate Category', 'Duplicate Category value found within Procedure Codes. Unable to Save, Try new Criteria!');
                } else {
                    insert();
                }
            }, null);
        }
    },
    updateByAltCat: {
        validate: function () {
            if ($("#AlternateCategory_UBAC_Old").data("kendoDropDownList").value() == '') {
                MICore.Notification.warning('Invalid Alternate category', 'Please select Alternate category!');
                return;
            }

            if ($("#AlternateCategory_UBAC_New").data("kendoDropDownList").value() == '') {
                MICore.Notification.warning('Invalid New Alternate category', 'Please select New Alternate category!');
                return;
            }

            //Confirm user that it will update all Old Categories with New but without those with ~ in Specialty
            MICore.Notification.question('Please confirm!',
                'Results in the selected category with a Specialty Tilde will not be updated.<br/>Do you want to proceed with this edit to all other results?', 'OK', 'Cancel', function () {

                    var categoryUpdateParam = {
                        P_PROC_CD: null,
                        P_DRUG_NM: null,
                        P_ALTERNATE_CATEGORY: $("#AlternateCategory_UBAC_Old").data("kendoDropDownList").value(),
                        P_ALTERNATE_SUB_CATEGORY: $("#AlternateSubCategory_UBAC_Old").data("kendoDropDownList").value()
                    }
                    MICore.Notification.whileSaving('Saving changes', function () {
                        // get epal records using this category
                        _categoryUpdateController.edit.getRecordsImpacted(categoryUpdateParam, updateAltCat, 'update by Alternate category');                        
                    });
                });

            function updateAltCat() {
                var categoryUpdate_Edit_Dto = {
                    P_PROC_CD: null,
                    P_DRUG_NM: null,
                    P_ALTERNATE_CATEGORY_OLD: $("#AlternateCategory_UBAC_Old").data("kendoDropDownList").value(),
                    P_ALTERNATE_SUB_CATEGORY_OLD: $("#AlternateSubCategory_UBAC_Old").data("kendoDropDownList").value(),
                    P_ALTERNATE_CATEGORY_NEW: $("#AlternateCategory_UBAC_New").data("kendoDropDownList").value(),
                    P_ALTERNATE_SUB_CATEGORY_NEW: $("#AlternateSubCategory_UBAC_New").data("kendoDropDownList").value(),
                    P_LST_UPDT_BY: MS_ID,
                    P_LST_UPDT_DT: null
                };

                MICore.Api.post(MIApp.Common.ApiEPRepository.get('admin-update-alt-cat-url', 'CategoryUpdateUrls'), categoryUpdate_Edit_Dto, function (updateDto) {
                    if (updateDto.StatusType == 'Error')
                        MICore.Notification.error('Error while updating Alternate Category', updateDto.Message);
                    else {
                        MICore.Notification.success('Saved', updateDto.Message, function () {
                            $('#updateByAltCatModal').modal('hide');
                        });
                    }
                });
            }
        }
    },
    updateSearchResults: function () {
        var CategoryUpdate_Edit_Dtos = [];
        var selectedCategories = _categoryUpdateController.grid.getSelectedCategories();
        selectedCategories.forEach(function (sc) {
            CategoryUpdate_Edit_Dtos.push(
                {
                    P_PROC_CD: sc.P_PROC_CD,
                    P_DRUG_NM: sc.P_DRUG_NM,
                    P_ALTERNATE_CATEGORY_OLD: sc.P_ALTERNATE_CATEGORY_OLD,
                    P_ALTERNATE_SUB_CATEGORY_OLD: sc.P_ALTERNATE_SUB_CATEGORY_OLD,
                    P_ALTERNATE_CATEGORY_NEW: sc.P_ALTERNATE_CATEGORY_NEW,
                    P_ALTERNATE_SUB_CATEGORY_NEW: sc.P_ALTERNATE_SUB_CATEGORY_NEW,
                    P_LST_UPDT_BY: MS_ID,
                    P_LST_UPDT_DT: null
                }
            );
        });

        MICore.Api.post(MIApp.Common.ApiEPRepository.get('admin-update-search-category-results-url', 'CategoryUpdateUrls'), CategoryUpdate_Edit_Dtos, function (updateDto) {
            if (updateDto.StatusType = 'Error')
                MICore.Notification.error('Error while updating Alternate Category', updateDto.Message);
            else if (updateDto.StatusType = 'Warning')
                MICore.Notification.warning('Records not updated', updateDto.Message);
            else
                MICore.Notification.success('Saved', updateDto.Message);
        });
    },
    dropdown: {
        param: {
            ALTERNATE_CATEGORY: function () {
                return {
                    p_type: 'ALTERNATE_CATEGORY',
                    p_parent_value: null
                }
            },
            ALTERNATE_SUB_CATEGORY: function () {
                return {
                    p_type: 'ALTERNATE_SUB_CATEGORY',
                    p_parent_value: null//$('#AlternateCategory_Edit').val()
                }
            },
            ALTERNATE_CATEGORY_UBAC_OLD: function () {
                return {
                    text: $("#txtUBAC_OldAlternateCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_CATEGORY",
                    P_PARENT_CATEGORY: null,
                    P_DRUG_NM: null
                }
            },
            ALTERNATE_SUB_CATEGORY_UBAC_OLD: function () {
                return {
                    text: $("#txtUBAC_OldAlternateSubCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_SUB_CATEGORY",
                    P_PARENT_CATEGORY: $('#AlternateCategory_UBAC_Old').data("kendoDropDownList").value(),
                    P_DRUG_NM: null
                }
            },
            ALTERNATE_CATEGORY_UBAC_NEW: function () {
                return {
                    text: $("#txtUBAC_NewAlternateCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_CATEGORY",
                    P_PARENT_CATEGORY: null,
                    P_DRUG_NM: null
                }
            },
            ALTERNATE_SUB_CATEGORY_UBAC_NEW: function () {
                return {
                    text: $("#txtUBAC_NewAlternateSubCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_SUB_CATEGORY",
                    P_PARENT_CATEGORY: null,
                    P_DRUG_NM: null
                }
            }
        },
        onChange_UBAC_NewAlternateCategory: function () {
            $("#txtUBAC_NewAlternateCategory").val('');
            $("#txtUBAC_NewAlternateCategory").val($("#AlternateCategoryUBAC_New").val());
        },
        onChange_UBACNewAlternateSubCategory: function () {
            $("#txtUBAC_NewAlternateSubCategory").val('');
            $("#txtUBAC_NewAlternateSubCategory").val($("#AlternateSubCategoryUBAC_New").val());
        },
        onChange_UBAC_OldAlternateCategory: function () {
            $("#txtUBAC_OldAlternateCategory").val('');
            $("#txtUBAC_OldAlternateCategory").val($("#AlternateCategoryUBAC_Old").val());
        },
        onChange_UBAC_OldAlternateSubCategory: function () {
            $("#txtUBAC_OldAlternateSubCategory").val('');
            $("#txtUBAC_OldAlternateSubCategory").val($("#AlternateSubCategoryUBAC_Old").val());
        },
        onFiltering_UBAC_NewAlternateCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtUBAC_NewAlternateCategoryFilterText').val(filterText);
            }
        },
        onFiltering_UBAC_NewAlternateSubCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtUBAC_NewAlternateSubCategoryFilterText').val(filterText);
            }
        },
        onFiltering_UBAC_OldAlternateCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtUBAC_OldAlternateCategoryFilterText').val(filterText);
            }
        },
        onFiltering_UBAC_OldAlternateSubCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtUBAC_OldAlternateSubCategoryFilterText').val(filterText);
            }
        }
    },
    multiSelect: {
        param: {
            SPECIALTY: function () {
                return {
                    p_type: 'SPECIALTY',
                    p_parent_value: null
                }
            },
            ALTERNATE_CATEGORY: function () {
                return {
                    text: $("#txtAlternateCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_CATEGORY",
                    P_PARENT_CATEGORY: null,
                    P_DRUG_NM: null
                }
            },
            ALTERNATE_SUB_CATEGORY: function () {
                return {
                    text: $("#txtAlternateSubCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_SUB_CATEGORY",
                    P_PARENT_CATEGORY: $('#txtAlternateCategory').val(),
                    P_DRUG_NM: null
                }
            },
            NEWALTERNATE_CATEGORY: function () {
                return {
                    text: $("#txtNewAlternateCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_CATEGORY",
                    P_PARENT_CATEGORY: null,
                    P_DRUG_NM: $('#NewSpecialty').data("kendoDropDownList").value()
                }
            },
            NEWALTERNATE_SUB_CATEGORY: function () {
                return {
                    text: $("#txtNewAlternateSubCategoryFilterText").val(),
                    P_CATEGORY_TYPE: "ALTERNATE_SUB_CATEGORY",
                    P_PARENT_CATEGORY: $('#NewAlternateCategory').data("kendoDropDownList").value(),
                    P_DRUG_NM: $('#NewSpecialty').data("kendoDropDownList").value()
                }
            }

        },
        onChange_AlternateCategory: function () {
            $("#txtAlternateCategory").val('');
            $("#txtAlternateCategory").val($("#msAlternateCategory").val());
        },
        onChange_AlternateSubCategory: function () {
            $("#txtAlternateSubCategory").val('');
            $("#txtAlternateSubCategory").val($("#msAlternateSubCategory").val());
        },
        onChange_NewSpecialty: function () {
            $("#NewAlternateCategory").data("kendoDropDownList").dataSource.read();
            $("#NewAlternateCategory").data("kendoDropDownList").refresh();

            $("#NewAlternateSubCategory").data("kendoDropDownList").dataSource.read();
            $("#NewAlternateSubCategory").data("kendoDropDownList").refresh();
        },
        onChange_NewAlternateCategory: function () {
            $("#txtNewAlternateCategory").val('');
            $("#txtNewAlternateCategory").val($("#NewAlternateCategory").val());
        },
        onChange_NewAlternateSubCategory: function () {
            $("#txtNewAlternateSubCategory").val('');
            $("#txtNewAlternateSubCategory").val($("#NewAlternateSubCategory").val());
        },
        onFiltering_AlternateCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtAlternateCategoryFilterText').val(filterText);
            }
        },
        onFiltering_AlternateSubCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtAlternateSubCategoryFilterText').val(filterText);
            }
        },
        onFiltering_NewAlternateCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtNewAlternateCategoryFilterText').val(filterText);
            }
        },
        onFiltering_NewAlternateSubCategory: function (e) {
            if (e.filter) {
                var filterText = e.filter.value;
                $('#txtNewAlternateSubCategoryFilterText').val(filterText);
            }
        }
    },
    grid: {
        param: function () {
            return {
                P_PROC_CD: $('#txtProcedureCode').val().toUpperCase(),
                P_DRUG_NM: $("#msSpecialty").val() != null ? $("#msSpecialty").val().join(',') : '',
                P_ALTERNATE_CATEGORY: $("#msAlternateCategory").val() != null ? $("#msAlternateCategory").val().join(',') : '',
                P_ALTERNATE_SUB_CATEGORY: $("#msAlternateSubCategory").val() != null ? $("#msAlternateSubCategory").val().join(',') : ''
            }
        },
        refresh: function () {
            $("#grid_category_update").data("kendoGrid").dataSource.read();
            $("#grid_category_update").data("kendoGrid").refresh();
        },
        clear: function () {
            $("#grid_category_update").data("kendoGrid").dataSource.data([]);
        },
        onGridRequestEnd: function (e) {
            window.kendo.ui.progress($("#grid_category_update"), false);
        },
        onDataBound: function () {
            /*
            var grid = $("#grid_category_update").data("kendoGrid");
            var rows = grid.items();

            $(rows).each(function (e) {
                var row = this;
                var dataItem = grid.dataItem(row);

                if (dataItem.EDITABLE_IND == 'N') {
                    var checkbox = $(row).find("input[type=checkbox]");
                    $(checkbox).hide();
                    $(checkbox).next().hide();
                }
            })
            */
        },
        getSelectedCategories: function () {
            var selectedCategories = [];
            var grid = $("#grid_category_update").data("kendoGrid");
            grid.select().each(function () {
                selectedCategories.push(grid.dataItem(this));
            });
            return selectedCategories;
        },
        paramSelectionCheck: function () {
            if ($.trim($('#txtProcedureCode').val()).length > 0 ||
                $('#msSpecialty').val()?.length > 0 ||
                $('#msAlternateSubCategory').val()?.length > 0 ||
                $('#msAlternateCategory').val()?.length > 0) {
                _categoryUpdateController.grid.refresh();
            }
        }
    }
}

$(document).ready(function () {
    _categoryUpdateController.init();
});

function isNullOrEmpty(s) {
    return (s == null || s === "");
}