using Dapper;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.Domain.SharedFiles;
using Supermarket.Core.UseCases.ManagerMenu;
using Supermarket.Infrastructure.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Supermarket.Infrastructure.SharedFiles
{
    internal class SharedFileRepository : CrudRepositoryBase<SharedFile, int, DbSharedFile>, ISharedFileRepository
    {
        public SharedFileRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public async Task<PagedResult<ManagerMenuSharedFile>> GetAllAsync(int supermarketId, RecordsRange recordsRange, string? search)
        {
            var parameters = new DynamicParameters()
            .AddParameter("supermarket_id", supermarketId)
            .AddParameter("hledani", search);

            const string sql = @"SELECT 
                        s.soubor_id,
                        s.nazev_souboru,
                        s.pripona,
                        s.datum_nahrani,
                        s.supermarket_id,
                        s.zamestnanec_id,
                        s.datum_modifikace,
                        LISTAGG(z.jmeno || ' ' || z.prijmeni, ', ') WITHIN GROUP (ORDER BY z.jmeno, z.prijmeni) as zamestnanec_nazev
                    FROM
                        SOUBORY s
                    JOIN
                        ZAMESTNANCI z ON z.zamestnanec_id = s.zamestnanec_id
                    WHERE
                        s.supermarket_id = :supermarket_id AND s.nazev_souboru LIKE '%' || :hledani || '%'
                    GROUP BY
                        s.soubor_id,
                        s.nazev_souboru,
                        s.pripona,
                        s.datum_nahrani,
                        s.supermarket_id,
                        s.zamestnanec_id,
                        s.datum_modifikace";


            const string blobSql = @"SELECT s.data FROM SOUBORY s WHERE s.soubor_id = :soubor_id";


            var orderByColumns = DbSharedFile.IdentityColumns
            .Select(ic => $"s.{ic}");


            var result = await GetPagedResult<DbManagerMenuSharedFile>(recordsRange, sql, orderByColumns, parameters);

            foreach (var dbFile in result.Items)
            {
                var blobParameters = new DynamicParameters().AddParameter("soubor_id", dbFile.soubor_id);
                var blobData = await _oracleConnection.QuerySingleOrDefaultAsync<byte[]>(blobSql, blobParameters);
                if (blobData != null)
                {
                dbFile.data = blobData;

                }
            }

            return result.Select(dbFile => dbFile.ToDomainEntity());


            //var result = await GetPagedResult<DbManagerMenuSharedFile>(recordsRange, sql, orderByColumns, parameters);

            //return result.Select(dbFile => dbFile.ToDomainEntity());
        }

        public Task SaveSharedFile(SharedFile file)
        {
            throw new NotImplementedException();
        }
    }
}
