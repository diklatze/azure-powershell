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
    public class NewAzureFirewallPolicyDraftCommand : AzureFirewallPolicyBaseCmdlet
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
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The operation mode for Threat Intelligence.")]
        [ValidateSet(
            MNM.AzureFirewallThreatIntelMode.Alert,
            MNM.AzureFirewallThreatIntelMode.Deny,
            MNM.AzureFirewallThreatIntelMode.Off,
            IgnoreCase = false)]
        public string ThreatIntelMode { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The whitelist for Threat Intelligence")]
        public PSAzureFirewallPolicyThreatIntelWhitelist ThreatIntelWhitelist { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The base policy to inherit from")]
        public string BasePolicy { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The DNS Setting")]
        public PSAzureFirewallPolicyDnsSettings DnsSetting { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The SQL related setting")]
        public PSAzureFirewallPolicySqlSetting SqlSetting { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Do not ask for confirmation if you want to overwrite a resource")]
        public SwitchParameter Force { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Run cmdlet in the background")]
        public SwitchParameter AsJob { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The Intrusion Detection Setting")]
        [ValidateNotNull]
        public PSAzureFirewallPolicyIntrusionDetection IntrusionDetection { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The private IP ranges to which traffic won't be SNAT'ed"
        )]
        public string[] PrivateRange { get; set; }//Dikla Not sure

        public override void Execute()
        {

            base.Execute();


            /*            ConfirmAction(
                            Force.IsPresent,
                            string.Format(Properties.Resources.OverwritingResource, Name),
                            Properties.Resources.CreatingResourceMessage,
                            Name,
                            () => WriteObject(this.CreateAzureFirewallPolicy()),
                            () => present);*/
        }

        private PSAzureFirewallPolicyDraft CreateAzureFirewallPolicyDraft()
        {

            var policy = (this.AzureFirewallPolicy == null) ? GetAzureFirewallPolicy(this.ResourceGroupName, this.PolicyName) : this.AzureFirewallPolicy;
            this.ResourceGroupName = policy.ResourceGroupName;
            this.PolicyName = policy.Name;

            var firewallPolicyDraft = new PSAzureFirewallPolicyDraft()
            {
                RuleCollectionGroups = policy.RuleCollectionGroups,
                ThreatIntelMode = this.ThreatIntelMode ?? policy.ThreatIntelMode,
                ThreatIntelWhitelist = this.ThreatIntelWhitelist ?? policy.ThreatIntelWhitelist,
                BasePolicy = BasePolicy != null ? new Microsoft.Azure.Management.Network.Models.SubResource(BasePolicy) : policy.BasePolicy,
                DnsSettings = this.DnsSetting ?? policy.DnsSettings,
                SqlSetting = this.SqlSetting ?? policy.SqlSetting,
                IntrusionDetection = this.IntrusionDetection ?? policy.IntrusionDetection,
                PrivateRange = this.PrivateRange ?? policy.PrivateRange
            };

            // Map to the sdk object
            var azureFirewallPolicyDraftModel = NetworkResourceManagerProfile.Mapper.Map<MNM.FirewallPolicyDraft>(firewallPolicyDraft);

            // Execute the Create AzureFirewall call
            this.AzureFirewallPolicyClient.CreateOrUpdateDraft(this.ResourceGroupName, this.PolicyName, azureFirewallPolicyDraftModel);
            return this.GetAzureFirewallPolicyDraft(this.ResourceGroupName, this.PolicyName);
        }
    }
}
