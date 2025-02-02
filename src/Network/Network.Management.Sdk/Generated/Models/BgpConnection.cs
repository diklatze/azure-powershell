// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Network.Models
{
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Virtual Appliance Site resource.
    /// </summary>
    [Rest.Serialization.JsonTransformation]
    public partial class BgpConnection : SubResource
    {
        /// <summary>
        /// Initializes a new instance of the BgpConnection class.
        /// </summary>
        public BgpConnection()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the BgpConnection class.
        /// </summary>
        /// <param name="id">Resource ID.</param>
        /// <param name="peerAsn">Peer ASN.</param>
        /// <param name="peerIp">Peer IP.</param>
        /// <param name="hubVirtualNetworkConnection">The reference to the
        /// HubVirtualNetworkConnection resource.</param>
        /// <param name="provisioningState">The provisioning state of the
        /// resource. Possible values include: 'Succeeded', 'Updating',
        /// 'Deleting', 'Failed'</param>
        /// <param name="connectionState">The current state of the VirtualHub
        /// to Peer. Possible values include: 'Unknown', 'Connecting',
        /// 'Connected', 'NotConnected'</param>
        /// <param name="name">Name of the connection.</param>
        /// <param name="etag">A unique read-only string that changes whenever
        /// the resource is updated.</param>
        /// <param name="type">Connection type.</param>
        public BgpConnection(string id = default(string), long? peerAsn = default(long?), string peerIp = default(string), SubResource hubVirtualNetworkConnection = default(SubResource), string provisioningState = default(string), string connectionState = default(string), string name = default(string), string etag = default(string), string type = default(string))
            : base(id)
        {
            PeerAsn = peerAsn;
            PeerIp = peerIp;
            HubVirtualNetworkConnection = hubVirtualNetworkConnection;
            ProvisioningState = provisioningState;
            ConnectionState = connectionState;
            Name = name;
            Etag = etag;
            Type = type;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets peer ASN.
        /// </summary>
        [JsonProperty(PropertyName = "properties.peerAsn")]
        public long? PeerAsn { get; set; }

        /// <summary>
        /// Gets or sets peer IP.
        /// </summary>
        [JsonProperty(PropertyName = "properties.peerIp")]
        public string PeerIp { get; set; }

        /// <summary>
        /// Gets or sets the reference to the HubVirtualNetworkConnection
        /// resource.
        /// </summary>
        [JsonProperty(PropertyName = "properties.hubVirtualNetworkConnection")]
        public SubResource HubVirtualNetworkConnection { get; set; }

        /// <summary>
        /// Gets the provisioning state of the resource. Possible values
        /// include: 'Succeeded', 'Updating', 'Deleting', 'Failed'
        /// </summary>
        [JsonProperty(PropertyName = "properties.provisioningState")]
        public string ProvisioningState { get; private set; }

        /// <summary>
        /// Gets the current state of the VirtualHub to Peer. Possible values
        /// include: 'Unknown', 'Connecting', 'Connected', 'NotConnected'
        /// </summary>
        [JsonProperty(PropertyName = "properties.connectionState")]
        public string ConnectionState { get; private set; }

        /// <summary>
        /// Gets or sets name of the connection.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets a unique read-only string that changes whenever the resource
        /// is updated.
        /// </summary>
        [JsonProperty(PropertyName = "etag")]
        public string Etag { get; private set; }

        /// <summary>
        /// Gets connection type.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (PeerAsn != null)
            {
                if (PeerAsn > 4294967295)
                {
                    throw new ValidationException(ValidationRules.InclusiveMaximum, "PeerAsn", 4294967295);
                }
                if (PeerAsn < 0)
                {
                    throw new ValidationException(ValidationRules.InclusiveMinimum, "PeerAsn", 0);
                }
            }
        }
    }
}
