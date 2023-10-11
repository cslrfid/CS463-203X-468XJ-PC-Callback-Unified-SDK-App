////////////////////////////////////////////////////////////////////////////////
//  INTEL CONFIDENTIAL
//  Copyright 2006, 2007 Intel Corporation All Rights Reserved.
//
//  The source code contained or described herein and all documents related to
//  the source code ("Material") are owned by Intel Corporation or its suppliers
//  or licensors. Title to the Material remains with Intel Corporation or its
//  suppliers and licensors. The Material may contain trade secrets and
//  proprietary and confidential information of Intel Corporation and its
//  suppliers and licensors, and is protected by worldwide copyright and trade
//  secret laws and treaty provisions. No part of the Material may be used,
//  copied, reproduced, modified, published, uploaded, posted, transmitted,
//  distributed, or disclosed in any way without Intel's prior express written
//  permission. 
//  
//  No license under any patent, copyright, trade secret or other intellectual
//  property right is granted to or conferred upon you by disclosure or delivery
//  of the Materials, either expressly, by implication, inducement, estoppel or
//  otherwise. Any license under such intellectual property rights must be
//  express and approved by Intel in writing.
//
//  Unless otherwise agreed by Intel in writing, you may not remove or alter
//  this notice or any other notice embedded in Materials by Intel or Intel's
//  suppliers or licensors in any way.
////////////////////////////////////////////////////////////////////////////////



using System;
using System.Collections.Generic;
using System.Text;



namespace CSLibrary
{
    /// <summary>
    /// LinkProfile Information
    /// </summary>
    public class LinkProfileInfo
    {

        private CSLibrary.Structures.RadioLinkProfile linkProfile;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="linkProfile"></param>
        public LinkProfileInfo
        (
            CSLibrary.Structures.RadioLinkProfile linkProfile
        )
        {
            // Currently just reference copy ~ change to deep 
            // copy later or ?

            this.linkProfile = linkProfile;
        }

        /// <summary>
        /// Enable current profile
        /// </summary>
        public Boolean Enabled
        {
            get { return ( this.linkProfile.enabled == 0 ) ? false : true; }

            set { this.linkProfile.enabled = ( UInt32 ) ( ( value == false ) ? 0 : 1 ); }
        }
        /// <summary>
        /// LinkProfile ID
        /// </summary>
        public UInt64 ProfileId
        {
            get { return this.linkProfile.profileId; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.profileId = value; }
        }
        /// <summary>
        /// LinkProfile Version
        /// </summary>
        public UInt32 ProfileVersion
        {
            get { return this.linkProfile.profileVersion; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.profileVersion = value; }
        }
        /// <summary>
        /// LinkProfile UID
        /// </summary>
        public String ProfileUniqueId
        {
            get
            {
                // Generated 'field' where profile unique identifier
                // is documented as the profileId + profileVersion

                return String.Format( "{0}:{1}", ProfileId, ProfileVersion );
            }
        }
        /// <summary>
        /// LinkPrile Protocol
        /// </summary>
        public CSLibrary.Constants.RadioProtocol ProfileProtocol
        {
            get { return this.linkProfile.profileProtocol; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.profileProtocol = value; }
        }
        /// <summary>
        /// Dense Reader Mode enable
        /// </summary>
        public Boolean DenseReaderMode
        {
            get { return ( this.linkProfile.denseReaderMode == 0 ) ? false : true; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.denseReaderMode = ( UInt32 ) ( ( value == false ) ? 0 : 1 ); }
        }
        /// <summary>
        /// Wideband Rssi Samples
        /// </summary>
        public UInt32 WidebandRssiSamples
        {
            get { return this.linkProfile.widebandRssiSamples; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.widebandRssiSamples = value; }
        }
        /// <summary>
        /// Narrowband Rssi Samples
        /// </summary>
        public UInt32 NarrowbandRssiSamples
        {
            get { return this.linkProfile.narrowbandRssiSamples; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.narrowbandRssiSamples = value; }
        }
        /// <summary>
        /// Realtime Rssi Enabled
        /// </summary>
        public Boolean RealtimeRssiEnabled
        {
            get { return ( this.linkProfile.realtimeRssiEnabled == 0 ) ? false : true; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.realtimeRssiEnabled = ( UInt32 ) ( ( value == false ) ? 0 : 1 ); }
        }
        /// <summary>
        /// Realtime Wideband Rssi Samples
        /// </summary>
        public UInt32 RealtimeWidebandRssiSamples
        {
            get { return this.linkProfile.realtimeWidebandRssiSamples; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.realtimeWidebandRssiSamples = value; }
        }
        /// <summary>
        /// Realtime Narrowband Rssi Samples
        /// </summary>
        public UInt32 RealtimeNarrowbandRssiSamples
        {
            get { return this.linkProfile.realtimeNarrowbandRssiSamples; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.realtimeNarrowbandRssiSamples = value; }
        }


        // TODO: Wrap the union classes so can be visually represented
        /// <summary>
        /// 
        /// </summary>
        public CSLibrary.Structures.RadioLinkProfileConfig LinkProfileConfig
        {
            get { return (CSLibrary.Structures.RadioLinkProfileConfig)this.linkProfile.profileConfig; }

            // NO IMPL ON RADIO 
            // set { this.linkProfile.profileConfig = value; }
        }
        /// <summary>
        /// Profile Name
        /// </summary>
        public string Name
        {
            // Since only 1 possibility in current config union, hard
            // coding the casting to it... TODO: make dynamic

            get
            {
                return string.Format
                    (
                        "{0} / M{1} / {2} khz",
                        ((CSLibrary.Structures.RadioLinkProfileConfig)this.linkProfile.profileConfig).modulationType,
                        (UInt32)((CSLibrary.Structures.RadioLinkProfileConfig)this.linkProfile.profileConfig).millerNumber,
                        ( int ) Math.Round
                            (
                                ((double)((CSLibrary.Structures.RadioLinkProfileConfig)this.linkProfile.profileConfig).trLinkFrequency) / 1000.0, 0
                            )
                    );
            }
        }
        /// <summary>
        /// Profile Description
        /// </summary>
        public string Description
        {
            get
            {
                return Name;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString( )
        {
            return Name;
        }


    } // End class Source_LinkProfile


} // End namespace RFID.RFIDInterface
