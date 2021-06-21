using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using src.Models;
using src.Utils;

namespace src.Services
{
    public static class ExtratoService
    {
        public static async Task<ActionResult<ExpandoObject>> ListAll(devContext _context, IQueryCollection queryString)
        {
            try
            {
                dynamic retorno = new ExpandoObject();
                string where;
                List<MySqlConnector.MySqlParameter> param;
                Where(queryString, out where, out param);
                where = where.Length > 0 ? where.Substring(0, where.Length - 4) : where;

                string scriptE = @"SELECT * FROM dev.extrato " + where + @"";
                var result = await _context.Extratos
                                                .FromSqlRaw(scriptE, parameters: param.ToArray())
                                                .OrderBy(o => o.IdExtrato)
                                                .Select(r => Retorno(queryString, r))
                                                .ToListAsync();

                //retorno = result;
                retorno.CountRow = result.Count;
                retorno.Result = result;
                return retorno;


                if (result == null)
                    return null;

                return retorno;

            }
            catch (Exception ex)
            {
                dynamic erro = new ExpandoObject();
                erro.Message = ex.Message;
                return erro;
            }
        }


        public static async Task<ActionResult<ExpandoObject>> Saldo(devContext _context, IQueryCollection queryString)
        {

            try
            {
                dynamic retorno = new ExpandoObject();
                string where;
                List<MySqlConnector.MySqlParameter> param;
                Where(queryString, out where, out param);
                where = where.Length > 0 ? where.Substring(0, where.Length - 4) : where;

                string scriptE = @"SELECT * FROM dev.extrato " + where + @"";
                var resultCredit = await _context.Extratos
                                                .FromSqlRaw(scriptE, parameters: param.ToArray())
                                                .OrderBy(o => o.IdExtrato)
                                                .Where(r => r.FlTipo.Equals("CREDIT"))
                                                .Select(r => Retorno(queryString, r))

                                                .ToListAsync();

                var resultDebit = await _context.Extratos
                                .FromSqlRaw(scriptE, parameters: param.ToArray())
                                .OrderBy(o => o.IdExtrato)
                                .Where(r => r.FlTipo.Equals("DEBIT"))
                                .Select(r => Retorno(queryString, r))

                                .ToListAsync();


                decimal credito = 0;
                foreach (var item in resultCredit)
                {
                    credito = credito + item.VlMovimentado;
                }

                decimal debito = 0;
                foreach (var item2 in resultDebit)
                {
                    debito = debito + item2.VlMovimentado;
                }

                retorno.saldo = credito - debito;

                if (resultCredit == null && resultDebit == null)
                    return null;

                return retorno;

            }
            catch (Exception ex)
            {
                dynamic erro = new ExpandoObject();
                erro.Message = ex.Message;
                return erro;
            }
        }

        private static dynamic GetRetorno(Extrato r)
        {
            dynamic expando = new ExpandoObject();

            expando.IdExtrato = r.IdExtrato;
            expando.UserId = r.UserId;
            expando.DtExtrato = Convert.ToDateTime(r.DtExtrato).ToString("dd/MM/yyyy");
            expando.FlTipo = r.FlTipo;
            expando.VlMovimentado = r.VlMovimentado;

            return expando;
        }
       
        private static dynamic Retorno(IQueryCollection queryString, Extrato r)
        {
            dynamic expando = new ExpandoObject();

            if (queryString.ContainsKey("col"))
            {
                StringValues queryCols;
                queryString.TryGetValue("col", out queryCols);
                string[] cols = queryCols.ToString().Split(',');

                foreach (string item in cols)
                {


                    int key = Convert.ToInt16(item);
                    CAMPOS filtroEnum = (CAMPOS)key;
                    switch (filtroEnum)
                    {
                        case CAMPOS.IdExtrato:
                            expando.IdExtrato = r.IdExtrato;
                            break;
                        case CAMPOS.UserId:
                            expando.UserId = r.UserId;
                            break;
                        case CAMPOS.DtExtrato:
                            expando.DtExtrato = Convert.ToDateTime(r.DtExtrato).ToString("dd/MM/yyyy");
                            break;
                        case CAMPOS.FlTipo:
                            expando.FlTipo = r.FlTipo;
                            break;
                        case CAMPOS.VlMovimentado:
                            expando.VlMovimentado = r.VlMovimentado;
                            break;
                    }
                }
            }
            else
            {
                expando.IdExtrato = r.IdExtrato;
                expando.UserId = r.UserId;
                expando.DtExtrato = Convert.ToDateTime(r.DtExtrato).ToString("dd/MM/yyyy");
                expando.FlTipo = r.FlTipo;
                expando.VlMovimentado = r.VlMovimentado;

            }

            return expando;
        }

        private static void Where(IQueryCollection queryString, out string where, out List<MySqlConnector.MySqlParameter> param)
        {
            if (queryString.Count > 0)
            {
                where = "" + Environment.NewLine + " WHERE ";
                param = new List<MySqlConnector.MySqlParameter>();

                foreach (KeyValuePair<string, StringValues> item in queryString)
                {
                    if (item.Key != "col")
                    {
                        int key = Convert.ToInt16(item.Key);
                        CAMPOS filtroEnum = (CAMPOS)key;
                        switch (filtroEnum)
                        {
                            case CAMPOS.IdExtrato:
                                Int32 idExtrato = Convert.ToInt32(item.Value);
                                param.Add(new MySqlConnector.MySqlParameter("@idExtrato", idExtrato));
                                where += Environment.NewLine + "IdExtrato like @idExtrato AND";
                                break;
                            case CAMPOS.UserId:
                                Int32 userId = Convert.ToInt32(item.Value);
                                param.Add(new MySqlConnector.MySqlParameter("@userId", userId));
                                where += Environment.NewLine + "UserId like @userId AND";
                                break;
                            case CAMPOS.DtExtrato:
                                if (item.Value[0].Contains("|")) // BETWEEN
                                {
                                    string[] busca = item.Value.ToString().Split('|');
                                    DateTime dtaIni = DateTime.ParseExact(busca[0] + " 00:00:00.000", "yyyyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    DateTime dtaFim = DateTime.ParseExact(busca[1] + " 23:59:00.000", "yyyyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    string dtInicio = Date.GetDate(dtaIni);
                                    string dtFim = Date.GetDate(dtaFim);
                                    param.Add(new MySqlConnector.MySqlParameter("@dtInicio", dtInicio));
                                    param.Add(new MySqlConnector.MySqlParameter("@dtFim", dtFim));
                                    where += Environment.NewLine + "dtExtrato BETWEEN @dtInicio AND @dtFim AND";
                                }
                                else
                                {
                                    string busca = item.Value;
                                    DateTime dtExtrato = DateTime.ParseExact(busca + " 00:00:00.000", "yyyyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    param.Add(new MySqlConnector.MySqlParameter("@dtExtrato", dtExtrato)); //.ToString("yyyyMMdd HH:mm:ss.fff")
                                    where += Environment.NewLine + "dtExtrato = @dtExtrato AND";
                                }
                                break;
                            case CAMPOS.FlTipo:
                                string flTipo = Convert.ToString(item.Value);
                                if (flTipo.Contains("%"))
                                {
                                    string busca = flTipo.Replace("%", "").ToString();
                                    param.Add(new MySqlConnector.MySqlParameter("@flTipo", flTipo));
                                    where += Environment.NewLine + "flTipo LIKE '%@flTipo%' AND";
                                }
                                else
                                {
                                    param.Add(new MySqlConnector.MySqlParameter("@flTipo", flTipo));
                                    where += Environment.NewLine + "flTipo = @flTipo AND";
                                }
                                break;
                        }
                    }
                }
            }
            else {
                where = "";
                param = new List<MySqlConnector.MySqlParameter>();
            }
        }

        public enum CAMPOS
        {
            IdExtrato = 101,
            UserId = 102,
            DtExtrato = 103,
            FlTipo = 104,
            VlMovimentado = 105
        }


    }
}
