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
    public static class UserService
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

                string script = @"SELECT * FROM dev.user " + where + @"";
                var result = await _context.Users//.ToListAsync();//AsQueryable() //.Users.ToListAsync();
                                                .FromSqlRaw(script, parameters: param.ToArray())
                                                .OrderBy(o => o.Id)
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

        public static async Task<ActionResult<ExpandoObject>> Create(devContext _context, User _object)
        {
            try
            {
                dynamic retorno = new ExpandoObject();
                User userExist = _context.Users.Where(x => x.Email == _object.Email || x.UserName == _object.UserName).FirstOrDefault();
                if (userExist != null)
                {
                    retorno.Message = "Usuário ou e-mail existente";
                    retorno.Result = false;
                    return retorno;
                }
                else
                {
                    var obj = await _context.Users.AddAsync(_object);
                    _context.SaveChanges();
                    retorno.Result = true;
                    retorno.Message = "Novo usuário criado";
                    return retorno;
                }
                
                    
            }
            catch (Exception ex)
            {
                dynamic erro = new ExpandoObject();
                erro.Message = ex.Message;
                erro.Result = false;
                return erro;
            }
        }

        private static dynamic GetRetorno(User r)
        {
            dynamic expando = new ExpandoObject();

            expando.Id = r.Id;
            expando.IsActive = r.IsActive.Equals(1) ? true : false;
            expando.CreatedDate = Convert.ToDateTime(r.CreatedDate).ToString("dd/MM/yyyy");
            expando.UserName = r.UserName;
            expando.Password = "*********";//r.Password;
            expando.RealName = r.RealName;
            expando.NumCpf = r.NumCpf;
            expando.Email = r.Email;
            expando.Phone = r.Phone;

            return expando;
        }
       
        private static dynamic Retorno(IQueryCollection queryString, User r)
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
                        case CAMPOS.Id:
                            expando.Id = r.Id;
                            break;
                        case CAMPOS.IsActive:
                            expando.IsActive = r.IsActive.Equals(1) ? true : false;
                            break;
                        case CAMPOS.CreatedDate:
                            expando.CreatedDate = Convert.ToDateTime(r.CreatedDate).ToString("dd/MM/yyyy");
                            break;
                        case CAMPOS.UserName:
                            expando.UserName = r.UserName;
                            break;
                        case CAMPOS.Password:
                            expando.Password = "*********";//r.Password;
                            break;
                        case CAMPOS.RealName:
                            expando.RealName = r.RealName;
                            break;
                        case CAMPOS.NumCpf:
                            expando.NumCpf = r.NumCpf;
                            break;
                        case CAMPOS.Email:
                            expando.Email = r.Email;
                            break;
                        case CAMPOS.Phone:
                            expando.Phone = r.Phone;                            
                            break;
                    }
                }
            }
            else
            {
                expando.Id = r.Id;
                expando.IsActive = r.IsActive.Equals(1) ? true : false;
                expando.CreatedDate = Convert.ToDateTime(r.CreatedDate).ToString("dd/MM/yyyy");
                expando.UserName = r.UserName;
                expando.Password = "*********";//r.Password;
                expando.RealName = r.RealName;
                expando.NumCpf = r.NumCpf;
                expando.Email = r.Email;
                expando.Phone = r.Phone;
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
                            case CAMPOS.Id:
                                Int32 id = Convert.ToInt32(item.Value);
                                param.Add(new MySqlConnector.MySqlParameter("@id", id));
                                where += Environment.NewLine + "id like @id AND";
                                break;
                            case CAMPOS.IsActive:
                                Int16 isActive = Convert.ToInt16(item.Value);
                                param.Add(new MySqlConnector.MySqlParameter("@isActive", isActive));
                                where += Environment.NewLine + "isActive like @isActive AND";
                                break;
                            case CAMPOS.CreatedDate:
                                if (item.Value[0].Contains("|")) // BETWEEN
                                {
                                    string[] busca = item.Value.ToString().Split('|');
                                    DateTime dtaIni = DateTime.ParseExact(busca[0] + " 00:00:00.000", "yyyyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    DateTime dtaFim = DateTime.ParseExact(busca[1] + " 23:59:00.000", "yyyyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    string dtInicio = Date.GetDate(dtaIni);
                                    string dtFim = Date.GetDate(dtaFim);
                                    param.Add(new MySqlConnector.MySqlParameter("@dtInicio", dtInicio));
                                    param.Add(new MySqlConnector.MySqlParameter("@dtFim", dtFim));
                                    where += Environment.NewLine + "createdDate BETWEEN @dtInicio AND @dtFim AND";
                                }
                                else
                                {
                                    string busca = item.Value;
                                    DateTime createdDate = DateTime.ParseExact(busca + " 00:00:00.000", "yyyyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                    param.Add(new MySqlConnector.MySqlParameter("@dtTitulo", createdDate)); //.ToString("yyyyMMdd HH:mm:ss.fff")
                                    where += Environment.NewLine + "createdDate = @createdDate AND";
                                }
                                break;
                            case CAMPOS.UserName:
                                string userName = Convert.ToString(item.Value);
                                if (userName.Contains("%"))
                                {
                                    string busca = userName.Replace("%", "").ToString();
                                    param.Add(new MySqlConnector.MySqlParameter("@userName", userName));
                                    where += Environment.NewLine + "userName LIKE '%@userName%' AND";
                                }
                                else
                                {
                                    param.Add(new MySqlConnector.MySqlParameter("@userName", userName));
                                    where += Environment.NewLine + "userName = @userName AND";
                                }
                                break;
                            case CAMPOS.NumCpf:
                                Int32 numCpf = Convert.ToInt32(item.Value);
                                param.Add(new MySqlConnector.MySqlParameter("@numCpf", numCpf));
                                where += Environment.NewLine + "numCpf = @numCpf AND";
                                break;
                            case CAMPOS.Email:
                                string email = Convert.ToString(item.Value);
                                if (email.Contains("%"))
                                {
                                    string busca = email.Replace("%", "").ToString();
                                    param.Add(new MySqlConnector.MySqlParameter("@email", busca));
                                    where += Environment.NewLine + "email LIKE '%@email%' AND";
                                }
                                else
                                {
                                    param.Add(new MySqlConnector.MySqlParameter("@email", email));
                                    where += Environment.NewLine + "email = @email AND";
                                }
                                break;
                            case CAMPOS.Phone:
                                string phone = Convert.ToString(item.Value);
                                if (phone.Contains("%"))
                                {
                                    string busca = phone.Replace("%", "").ToString();
                                    param.Add(new MySqlConnector.MySqlParameter("@phone", busca));
                                    where += Environment.NewLine + "phone LIKE '%@phone%' AND";
                                }
                                else
                                {
                                    param.Add(new MySqlConnector.MySqlParameter("@phone", phone));
                                    where += Environment.NewLine + "phone = @phone AND";
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
            Id = 100,
            IsActive = 101,
            CreatedDate = 102,
            UserName = 103,
            Password = 104,
            RealName = 105,
            NumCpf = 106,
            Email = 107,
            Phone = 108
        };


    }
}
