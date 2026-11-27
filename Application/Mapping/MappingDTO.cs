using AutoMapper;
using ModeloBackEndAPI.Application.DTOs;
using ModeloBackEndAPI.Domain.Model;

namespace ModeloBackEndAPI.Application.Mapping
{
    /// <summary>
    /// Configura os mapeamentos entre entidades e DTOs.
    /// </summary>
    public class MappingDTO : Profile
    {
        /// <summary>
        /// Inicializa uma nova instância de <c>Mapping DTO</c>.
        /// </summary>
        public MappingDTO()
        {
            CreateMap<Conta, ContaDTO>()
                .ForMember(conta => conta.Relacionamento, m => m.MapFrom(contadto => contadto.Relacionamento.ToString()))
                .ForMember(conta => conta.Situacao, m => m.MapFrom(contadto => contadto.Situacao.ToString()))
                .ForMember(conta => conta.LocalArquivo, m => m.MapFrom(contadto => GetFileName(contadto.LocalArquivo)))
                .ReverseMap();

            CreateMap<Transacao, TransacaoDTO>()
                .ForMember(transacao => transacao.Tipo, m => m.MapFrom(transacaodto => transacaodto.Tipo.ToString()))
                .ReverseMap();

            CreateMap<Usuario, UsuarioDTO>()
                .ReverseMap();

            //CreateMap<Usuario, ContaDTO>()
            //.ForMember(usuario => usuario.Id, m => m.MapFrom(contaDTO => contaDTO.Id))
            //.ForMember(usuario => usuario.NomeCompleto, m => m.MapFrom(contaDTO => contaDTO.Nome))
            //.ForMember(usuario => usuario.DtInclusao, m => m.MapFrom(contaDTO => contaDTO.DtInclusao))
            //.ForMember(usuario => usuario.DtExclusao, m => m.MapFrom(contaDTO => contaDTO.DtExclusao))
            //.ForMember(usuario => usuario.DtAlteracao, m => m.MapFrom(contaDTO => contaDTO.DtAlteracao))
            //.ForAllMembers(opt => opt.Condition((contaDTO, usuario, srcMember) => srcMember != null));
        }

        private static string GetFileName(string? localArquivo)
        {
            if (string.IsNullOrEmpty(localArquivo))
            {
                return string.Empty;
            }
            return localArquivo.Split("\\").Last();
        }
    }
}