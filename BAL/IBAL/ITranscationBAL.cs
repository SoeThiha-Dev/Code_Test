using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace WebAPI.BAL
{
    public interface ITranscationBAL : IGenericBAL<Transcation>
    {
        Task<bool> CreateTranscation(TranscationRequest request);
    }
}
