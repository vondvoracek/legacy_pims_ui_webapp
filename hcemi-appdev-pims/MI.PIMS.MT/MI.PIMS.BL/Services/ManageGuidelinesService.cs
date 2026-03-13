using MI.PIMS.BL.Repositories;
using MI.PIMS.BL.Services.Interfaces;
using MI.PIMS.BO.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.BL.Services
{

    public class ManageGuidelinesService: IManageGuidelinesService
    {
        private readonly ManageGuidelinesRepository _repository;

        public ManageGuidelinesService(ManageGuidelinesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DPOC_Inv_Gdln_Rules_PND_V_Admin_Dto>> GetGuidelinesByParams(
            string proc_cd, string iq_reference, string iq_gdln_id, string iq_gdln_version)
        {
            return await _repository.GetGuidelinesByParams(proc_cd, iq_reference, iq_gdln_id, iq_gdln_version);
        }

        public async Task<List<Ref_Procedures_V_Dto>> GetActiveProcedureCodes(string proc_cds)
        {
            var data = await _repository.GetActiveProcedureCodes(proc_cds);
            return data;
        }
    }

    public interface IManualGuidelineService
    {
        Task<(List<string> Inserted, List<string> Invalid, List<string> Duplicate)> AddGuidelinesAsync(
            string iq_gdln_id, string procCodesCsv, string iq_version, string iq_reference);
    }

    public class ManualGuidelineService : IManualGuidelineService
    {
        private readonly IManualGuidelineRepository _repository;

        public ManualGuidelineService(IManualGuidelineRepository repository)
        {
            _repository = repository;
        }

        public async Task<(List<string> Inserted, List<string> Invalid, List<string> Duplicate)> AddGuidelinesAsync(
            string iq_gdln_id, string procCodesCsv, string iq_version, string iq_reference)
        {
            var insertedRecords = new List<string>();
            var invalidProcCodes = new List<string>();
            var duplicateProcCodes = new List<string>();

            var procCodes = procCodesCsv.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(x => x.Trim())
                                        .Distinct()
                                        .ToList();

            foreach (var procCode in procCodes)
            {

                bool existsInEsync = await _repository.ExistsAsync(iq_gdln_id, procCode); // Your existing check
                bool existsInRefProc = await  _repository.ExistsInRefProceduresAsync(procCode); // New check

                if (!existsInRefProc)
                {
                    invalidProcCodes.Add(procCode);
                }
                else if (existsInEsync)
                {
                    duplicateProcCodes.Add(procCode);
                }
                else
                {
                    var guideline = new Ref_Iq_Manual_Guidelines_T
                    {
                        iq_gdln_id = iq_gdln_id,
                        iq_gdln_proc_cd = procCode,
                        iq_version = iq_version,
                        iq_reference = iq_reference
                    };

                    await _repository.AddAsync(guideline);
                    insertedRecords.Add(guideline.iq_gdln_proc_cd);
                }
            }

            return (insertedRecords, invalidProcCodes, duplicateProcCodes);
        }
    }
}
