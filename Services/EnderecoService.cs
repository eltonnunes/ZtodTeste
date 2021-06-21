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
    public static class EnderecoService
    {
        public static async Task<ActionResult<ExpandoObject>> Create(devContext _context, Endereco _object)
        {
            try
            {
                dynamic retorno = new ExpandoObject();
                Endereco enderecoExist = _context.Enderecos.Where(
                                                                x => x.NumCep == _object.NumCep &&
                                                                x.DsLogradouro == _object.DsLogradouro &&
                                                                x.Dsbairro == _object.Dsbairro &&
                                                                x.DsCidade == _object.DsCidade &&
                                                                x.DsEstado == _object.DsEstado &&
                                                                x.DsNumero == _object.DsNumero &&
                                                                x.UserId == _object.UserId
                                                              ).FirstOrDefault();
                if (enderecoExist != null)
                {
                    retorno.Message = "Endereço existente";
                    retorno.Result = false;
                    return retorno;
                }
                else
                {
                    var obj = await _context.Enderecos.AddAsync(_object);
                    _context.SaveChanges();
                    retorno.Result = true;
                    retorno.Message = "Novo Endereço criado";
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
    }
}
