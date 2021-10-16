using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Model;

namespace WebAPI.Repo
{
    public class TranscationRepo : GenericRepo<Transcation>, ITranscationRepo
    {
        public TranscationRepo(WebAPIDBContext dbContext)
                : base(dbContext)
        {
        }
    }
}
