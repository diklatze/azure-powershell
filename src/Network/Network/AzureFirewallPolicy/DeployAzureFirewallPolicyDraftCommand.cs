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

using System.Management.Automation;
using Microsoft.Azure.Commands.Network.Models;
using Microsoft.Azure.Management.Network;
using MNM = Microsoft.Azure.Management.Network.Models;

namespace Microsoft.Azure.Commands.Network
{
    [Cmdlet(VerbsCommon.New, ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "FirewallPolicy", SupportsShouldProcess = true), OutputType(typeof(PSAzureFirewallPolicy))]
    public class DeployAzureFirewallPolicyDraftCommand : AzureFirewallPolicyBaseCmdlet
    {

        [Alias("PolicyName")]
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The policy name.")]
        [ValidateNotNullOrEmpty]
        public virtual string PolicyName { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The resource group name.")]
        [ValidateNotNullOrEmpty]
        public virtual string ResourceGroupName { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Azure Firewall Policy.")]
        [ValidateNotNullOrEmpty]
        public virtual PSAzureFirewallPolicy AzureFirewallPolicy { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Do not ask for confirmation if you want to overwrite a resource")]
        public SwitchParameter Force { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Run cmdlet in the background")]
        public SwitchParameter AsJob { get; set; }

        public override void Execute()
        {

            base.Execute();
            var present = NetworkBaseCmdlet.IsResourcePresent(() => GetAzureFirewallPolicy(this.ResourceGroupName, this.PolicyName));
            ConfirmAction(
                Force.IsPresent,
                string.Format(Properties.Resources.OverwritingResource, PolicyName),
                Properties.Resources.CreatingResourceMessage,
                PolicyName,
                () => WriteObject(this.DeployAzureFirewallPolicyDraft()),
                () => present);
        }


        private PSAzureFirewallPolicyDraft DeployAzureFirewallPolicyDraft()
        {

            var policy = (this.AzureFirewallPolicy == null) ? GetAzureFirewallPolicy(this.ResourceGroupName, this.PolicyName) : this.AzureFirewallPolicy;
            this.ResourceGroupName = policy.ResourceGroupName;
            this.PolicyName = policy.Name;

            // Execute the Create AzureFirewall call
            this.AzureFirewallPolicyClient.DeployDraft(this.ResourceGroupName, this.PolicyName);
            return this.GetAzureFirewallPolicyDraft(this.ResourceGroupName, this.PolicyName);
        }
    }
}
