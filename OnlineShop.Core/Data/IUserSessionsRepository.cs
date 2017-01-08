﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IUserSessionsRepository
    {
        Task<int> GetActiveUserBySession(string session);
        Task CreateNewSession(int userId, string session);
        Task SetSessionExpired(string session);
    }
}
