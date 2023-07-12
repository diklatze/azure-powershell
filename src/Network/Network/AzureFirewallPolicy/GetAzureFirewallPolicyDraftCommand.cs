// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.Azure.Commands.Network.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using Microsoft.Azure.Management.Network;
using Microsoft.Azure.Management.Network.Models;
using Microsoft.Rest.Azure;
using Microsoft.WindowsAzure.Commands.Utilities.Common;

namespace Microsoft.Azure.Commands.Network
{
    [Cmdlet(VerbsCommon.Get, ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "FirewallPolicy", DefaultParameterSetName = GetByNameParameterSet), OutputType(typeof(PSAzureFirewallPolicy))]
    public class GetAzureFirewallPolicyDraftCommand : AzureFirewallPolicyBaseCmdlet
    {

        private const string GetByNameParameterSet = "GetByNameParameterSet";
        private const string GetByResourceIdParameterSet = "GetByResourceIdParameterSet";

        [Alias("ResourceName")]
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The resource name.", ParameterSetName = GetByNameParameterSet)]
        [ResourceNameCompleter("Microsoft.Network/azureFirewalls", "ResourceGroupName")]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public virtual string Name { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = false,
            HelpMessage = "The resource group name.", ParameterSetName = GetByNameParameterSet)]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public virtual string ResourceGroupName { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The resource Id.", ParameterSetName = GetByResourceIdParameterSet)]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public virtual string ResourceId { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Azure Firewall Policy.")]
        [ValidateNotNullOrEmpty]
        public virtual PSAzureFirewallPolicy AzureFirewallPolicy { get; set; }

        public override void ExecuteCmdlet()
        {
            if (AzureFirewallPolicy != null)
            {
                this.ResourceId = AzureFirewallPolicy.ResourceGroupName;
                this.Name = AzureFirewallPolicy.Name;
            }

            base.ExecuteCmdlet();
            if (this.IsParameterBound(c => c.Name))
            {
                var azureFirewallPolicyDraft = this.GetAzureFirewallPolicyDraft(this.ResourceGroupName, this.Name);
                WriteObject(azureFirewallPolicyDraft);
            }
            else if (this.IsParameterBound(c => c.ResourceId))
            {
                var resourceInfo = new ResourceIdentifier(ResourceId);
                ResourceGroupName = resourceInfo.ResourceGroupName;
                Name = resourceInfo.ResourceName;

                var azureFirewallPolicyDraft = this.GetAzureFirewallPolicyDraft(this.ResourceGroupName, this.Name);
                WriteObject(azureFirewallPolicyDraft);
            }
            
        }
    }
}
