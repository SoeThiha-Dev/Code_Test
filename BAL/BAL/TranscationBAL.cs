using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;
using WebAPI.Repo;

namespace WebAPI.BAL
{
    public class TranscationBAL : GenericBAL<Transcation>, ITranscationBAL
    {
        private readonly IGenericRepo<Transcation> _transcationRepo;

        public TranscationBAL(IGenericRepo<Transcation> transcationRepo)
            : base(transcationRepo)
        {
            _transcationRepo = transcationRepo;
        }

        public async Task<bool> CreateTranscation(TranscationRequest request)
        {
            try
            {
                //------------------------------Payment API------------------------------//
                //--------------------By using Card Info, Amount and PaymentID--------------------//
                var tranID = "UniqueTranscationID";
                Transcation transcation = new Transcation()
                {
                    TranscationID = tranID,
                    EVoucherID = request.EVoucherID,
                    PaymentID = request.PaymentID,
                    Amount = request.Amount,
                    PurchaseMobile = request.PurchaseMobile,
                    CreateDate = DateTime.UtcNow,
                    Flag = true
                };
                bool obj = await _transcationRepo.Create(transcation);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
