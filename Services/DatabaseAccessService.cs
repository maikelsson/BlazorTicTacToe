using DataAccessLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Services
{
    public class DatabaseAccessService
    {
        private IUserData _userDataDB;

        public DatabaseAccessService(IUserData userDataDB)
        {
            _userDataDB = userDataDB;
        }



    }
}
