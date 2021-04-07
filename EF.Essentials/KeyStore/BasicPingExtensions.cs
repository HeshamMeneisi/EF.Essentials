// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

using System.Threading;
using System.Threading.Tasks;
using EF.Essentials.KeyStore.Models;

namespace EF.Essentials.KeyStore
{
    /// <summary>
    /// Extension methods for BasicPing.
    /// </summary>
    public static partial class BasicPingExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static PingResponse Ping(this IBasicPing operations)
            {
                return operations.PingAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<PingResponse> PingAsync(this IBasicPing operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PingWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}