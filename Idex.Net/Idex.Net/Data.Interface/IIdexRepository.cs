using Idex.Net.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Idex.Net.Data.Interface
{
    public interface IIdexRepository
    {
        Task<Ticker> GetTicker(string pair);

        Task<Dictionary<string, Ticker[]>> GetTickers();
    }
}
