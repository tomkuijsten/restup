using Restup.DemoControllers.Model;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System.Collections.Generic;
using System.Linq;

namespace Restup.DemoControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class SimpleParameterControllerSample
    {
        /// <summary>
        /// Make sure the number of parameters in your UriFormat match the parameters in your method and
        /// the names (case sensitive) and order are respected.
        /// </summary>
        [UriFormat("/simpleparameter/{id}/property/{propName}")]
        public IGetResponse GetWithSimpleParameters(int id, string propName)
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new DataReceived()
                {
                    ID = id,
                    PropName = propName
                });
        }

        /// <summary>
        /// Make sure the number of parameters in your UriFormat match the parameters in your method and
        /// the names (case sensitive) and order are respected.
        /// </summary>
        [UriFormat("/simpleparameter/{id}/property/{propName}")]
        public IDeleteResponse DeleteWithSimpleParameters(int id, string propName)
        {
            return new DeleteResponse(DeleteResponse.ResponseStatus.OK);
        }

        /// <summary>
        /// Make sure the number of parameters in your UriFormat match the parameters in your method and
        /// the names (case sensitive) and order are respected.
        /// </summary>
        [UriFormat("/intparameterarray/{ids}")]
        public IGetResponse GetWithIntArrayParameters(IEnumerable<int> ids)
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new DataReceived()
                {
                    ID = ids.Sum()
                });
        }

        /// <summary>
        /// Make sure the number of parameters in your UriFormat match the parameters in your method and
        /// the names (case sensitive) and order are respected.
        /// </summary>
        [UriFormat("/doubleparameterarray/{ids}")]
        public IGetResponse GetWithDoubleArrayParameters(IEnumerable<double> ids)
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new DataReceived()
                {
                    ID = (int)ids.Sum()
                });
        }
    }
}
