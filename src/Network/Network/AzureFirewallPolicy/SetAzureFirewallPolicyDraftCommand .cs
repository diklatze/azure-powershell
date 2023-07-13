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

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.Azure.Commands.Network.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.Tags;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using Microsoft.Azure.Management.Network;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using MNM = Microsoft.Azure.Management.Network.Models;

namespace Microsoft.Azure.Commands.Network
{
    [Cmdlet(VerbsCommon.Set, ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "FirewallPolicy", SupportsShouldProcess = true, DefaultParameterSetName = SetByNameParameterSet), OutputType(typeof(PSAzureFirewallPolicy))]
    public class SetAzureFirewallPolicyDraftCommand : AzureFirewallPolicyBaseCmdlet
    {

        private const string SetByNameParameterSet = "SetByNameParameterSet";
        private const string SetByInputObjectParameterSet = "SetByInputObjectParameterSet";
        private const string SetByResourceIdParameterSet = "SetByResourceIdParameterSet";

        [Alias("PolicyName")]
        [Parameter(
           Mandatory = true,
           ValueFromPipelineByPropertyName = true,
           HelpMessage = "The policy name.", ParameterSetName = SetByNameParameterSet)]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public virtual string PolicyName { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The resource group name.", ParameterSetName = SetByNameParameterSet)]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public virtual string ResourceGroupName { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "The AzureFirewall Policy", ParameterSetName = SetByInputObjectParameterSet)]
        public PSAzureFirewallPolicy AzureFirewallPolicy { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background")]
        public SwitchParameter AsJob { get; set; }

        [Parameter(
                    Mandatory = true,
                    ValueFromPipelineByPropertyName = true,
                    HelpMessage = "The resource Id.", ParameterSetName = SetByResourceIdParameterSet)]
        [ValidateNotNullOrEmpty]
        [SupportsWildcards]
        public virtual string ResourceId { get; set; }

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
            HelpMessage = "The Intrusion Detection Setting")]
        [ValidateNotNull]
        public PSAzureFirewallPolicyIntrusionDetection IntrusionDetection { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The Private IP Range")]
        public string[] PrivateRange { get; set; }

        public override void Execute()
        {
            base.Execute();

            if (this.IsParameterBound(c => c.ResourceId))
            {
                var resourceInfo = new ResourceIdentifier(ResourceId);
                ResourceGroupName = resourceInfo.ResourceGroupName;
                PolicyName = resourceInfo.ResourceName;
            }
            else if (this.IsParameterBound(c => c.AzureFirewallPolicy))
            {
                ResourceGroupName = AzureFirewallPolicy.ResourceGroupName;
                PolicyName = AzureFirewallPolicy.Name;
            }

            if (!NetworkBaseCmdlet.IsResourcePresent(() => GetAzureFirewallPolicyDraft(ResourceGroupName, PolicyName)))
            {
                throw new ArgumentException(Microsoft.Azure.Commands.Network.Properties.Resources.ResourceNotFound);
            }

            var azureFirewallPolicyDraft = GetAzureFirewallPolicyDraft(ResourceGroupName, PolicyName);

            this.ThreatIntelMode = this.IsParameterBound(c => c.ThreatIntelMode) ? ThreatIntelMode : azureFirewallPolicyDraft.ThreatIntelMode;
            this.ThreatIntelWhitelist = this.IsParameterBound(c => c.ThreatIntelWhitelist) ? ThreatIntelWhitelist : azureFirewallPolicyDraft.ThreatIntelWhitelist;
            this.BasePolicy = this.IsParameterBound(c => c.BasePolicy) ? BasePolicy : (azureFirewallPolicyDraft.BasePolicy != null ? azureFirewallPolicyDraft.BasePolicy.Id : null);
            this.DnsSetting = this.IsParameterBound(c => c.DnsSetting) ? DnsSetting : (azureFirewallPolicyDraft.DnsSettings != null ? azureFirewallPolicyDraft.DnsSettings : null);
            this.SqlSetting = this.IsParameterBound(c => c.SqlSetting) ? SqlSetting : (azureFirewallPolicyDraft.SqlSetting != null ? azureFirewallPolicyDraft.SqlSetting : null);
            this.IntrusionDetection = this.IsParameterBound(c => c.IntrusionDetection) ? IntrusionDetection : (azureFirewallPolicyDraft.IntrusionDetection != null ? azureFirewallPolicyDraft.IntrusionDetection : null);
            this.PrivateRange = this.IsParameterBound(c => c.PrivateRange) ? PrivateRange : azureFirewallPolicyDraft.PrivateRange;
            
            var firewallPolicyDraft = new PSAzureFirewallPolicyDraft()
            {
                ResourceGroupName = this.ResourceGroupName,
                ThreatIntelMode = this.ThreatIntelMode ?? MNM.AzureFirewallThreatIntelMode.Alert,
                ThreatIntelWhitelist = this.ThreatIntelWhitelist,
                BasePolicy = this.BasePolicy != null ? new Microsoft.Azure.Management.Network.Models.SubResource(this.BasePolicy) : null,
                DnsSettings = this.DnsSetting,
                SqlSetting = this.SqlSetting,
                PrivateRange = this.PrivateRange
            };

            var azureFirewallPolicyDraftModel = NetworkResourceManagerProfile.Mapper.Map<MNM.FirewallPolicyDraft>(firewallPolicyDraft);

            // Execute the PUT AzureFirewall draft Policy call
            this.AzureFirewallPolicyClient.CreateOrUpdateDraft(ResourceGroupName, PolicyName, azureFirewallPolicyDraftModel);
            var getAzureFirewallPolicyDraft = this.GetAzureFirewallPolicyDraft(ResourceGroupName, PolicyName);
            WriteObject(getAzureFirewallPolicyDraft);

        }
    }
}
