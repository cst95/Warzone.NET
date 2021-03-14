using System;
using System.Threading;
using System.Threading.Tasks;
using Warzone.Exceptions;
using Warzone.Models;
using Warzone.Models.CodApi;

namespace Warzone
{
    public interface IWarzoneClient
    {
        /// <summary>
        /// Log into the Call of Duty API with a valid account before attempting to access other endpoints.
        /// Login valid for approximately two hours, after which point you will be required to re-login with this method.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> LoginAsync(string email, string password);
        
        /// <summary>
        ///  Fetch summarized data for the user's last twenty Warzone matches.
        ///  Only includes standard BR matches (Solo, Duos, Trios, Quads).
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="platform"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotLoggedInException">You're login has expired and you should reauthenticate with the LoginAsync method.</exception>
        /// <exception cref="WarzoneException">A generic catch all exception indicating something went wrong.</exception>
        /// <returns></returns>
        Task<WarzoneResponse<Summaries>> GetLastTwentyWarzoneMatchesAsync(string playerName, string platform,
            CancellationToken? cancellationToken);

        /// <summary>
        ///  Fetch summarized data for the user's Warzone games between the start and end date params.
        ///  Only includes standard BR matches (Solo, Duos, Trios, Quads).
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="platform"></param>
        /// <param name="startTime">If null is passed then set to Now - 24 hours.</param>
        /// <param name="endTime">If null is passed then set to Now.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotLoggedInException">You're login has expired and you should reauthenticate with the LoginAsync method.</exception>
        /// <exception cref="WarzoneException">A generic catch all exception indicating something went wrong.</exception>
        /// <returns></returns>
        Task<WarzoneResponse<Summaries>> GetWarzoneMatchesAsync(string playerName, string platform,
            DateTime? startTime, DateTime? endTime, CancellationToken? cancellationToken);
    }
}