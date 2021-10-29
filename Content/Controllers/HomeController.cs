﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;
using NethereumFaucet.ViewModel;
using Microsoft.Extensions.Options;

namespace NethereumFaucet.Controllers
{
    public class HomeController : Controller
    {
        FaucetSettings settings;
        
        public HomeController(IOptions<FaucetSettings> settings)
        {
            this.settings = settings.Value;
        }

        public IActionResult Index()
        {
            return View(new FaucetViewModel{Amount = this.settings.AmountToFund});
        }

        [HttpPost]
        public async Task<ActionResult> Index(FaucetViewModel faucetViewModel)
        {
            if (ModelState.IsValid)
            {
                var account = new Account(settings.FunderPrivateKey, settings.ChainId);
                var web3 = new Web3(account, settings.EthereumAddress);
                web3.TransactionManager.UseLegacyAsDefault = true;  // as long as EIP-1559 is not activated

                //var balance = await web3.Eth.GetBalance.SendRequestAsync(faucetViewModel.Address);
                //if (Web3.Convert.FromWei(balance.Value) > settings.MaxAmountToFund)
                //{
                //    ModelState.AddModelError("address", "Account cannot be funded, already has more than " + settings.MaxAmountToFund + " ether");
                //    return View(faucetViewModel);
                //}

                var txnHash = await web3.Eth.TransactionManager.SendTransactionAsync(account.Address, faucetViewModel.Address, new HexBigInteger(Web3.Convert.ToWei(faucetViewModel.Amount)));
                faucetViewModel.TransactionHash = txnHash;
                //faucetViewModel.Amount = settings.AmountToFund;
                
                return View(faucetViewModel);
            }
            else
            {
                return View("Index");
            }
        }

    }
}
