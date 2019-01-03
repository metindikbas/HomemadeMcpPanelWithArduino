namespace MDHomeCockpit.ListenerService.FsuipcData.Pmdg737
{
    internal class Pmdg737Events
    {
        // Overhead - Electric  
        public const uint THIRD_PARTY_EVENT_ID_MIN = 0x00011000; // 69632
        // MCP
        //
        public const uint EVT_MCP_COURSE_SELECTOR_L = (THIRD_PARTY_EVENT_ID_MIN + 376);
        public const uint EVT_MCP_FD_SWITCH_L = (THIRD_PARTY_EVENT_ID_MIN + 378);
        public const uint EVT_MCP_AT_ARM_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 380);
        public const uint EVT_MCP_N1_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 381);
        public const uint EVT_MCP_SPEED_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 382);
        public const uint EVT_MCP_CO_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 383);
        public const uint EVT_MCP_SPEED_SELECTOR = (THIRD_PARTY_EVENT_ID_MIN + 384);
        public const uint EVT_MCP_VNAV_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 386);
        public const uint EVT_MCP_SPD_INTV_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 387); // 70019
        public const uint EVT_MCP_BANK_ANGLE_SELECTOR = (THIRD_PARTY_EVENT_ID_MIN + 389); // 70021
        public const uint EVT_MCP_HEADING_SELECTOR = (THIRD_PARTY_EVENT_ID_MIN + 390); // 70022
        public const uint EVT_MCP_LVL_CHG_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 391); // 70023
        public const uint EVT_MCP_HDG_SEL_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 392);
        public const uint EVT_MCP_APP_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 393);
        public const uint EVT_MCP_ALT_HOLD_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 394);
        public const uint EVT_MCP_VS_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 395);
        public const uint EVT_MCP_VOR_LOC_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 396);
        public const uint EVT_MCP_LNAV_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 397);
        public const uint EVT_MCP_ALTITUDE_SELECTOR = (THIRD_PARTY_EVENT_ID_MIN + 400);
        public const uint EVT_MCP_VS_SELECTOR = (THIRD_PARTY_EVENT_ID_MIN + 401);
        public const uint EVT_MCP_CMD_A_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 402);
        public const uint EVT_MCP_CMD_B_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 403);
        public const uint EVT_MCP_CWS_A_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 404);
        public const uint EVT_MCP_CWS_B_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 405);
        public const uint EVT_MCP_DISENGAGE_BAR = (THIRD_PARTY_EVENT_ID_MIN + 406);
        public const uint EVT_MCP_FD_SWITCH_R = (THIRD_PARTY_EVENT_ID_MIN + 407);
        public const uint EVT_MCP_COURSE_SELECTOR_R = (THIRD_PARTY_EVENT_ID_MIN + 409);
        public const uint EVT_MCP_ALT_INTV_SWITCH = (THIRD_PARTY_EVENT_ID_MIN + 885);

        // Overhead - LIGHTS Panel
        public const uint EVT_OH_LAND_LIGHTS_GUARD = (THIRD_PARTY_EVENT_ID_MIN + 110);
        public const uint EVT_OH_LIGHTS_L_RETRACT = (THIRD_PARTY_EVENT_ID_MIN + 111);
        public const uint EVT_OH_LIGHTS_R_RETRACT = (THIRD_PARTY_EVENT_ID_MIN + 112);
        public const uint EVT_OH_LIGHTS_L_FIXED = (THIRD_PARTY_EVENT_ID_MIN + 113);
        public const uint EVT_OH_LIGHTS_R_FIXED = (THIRD_PARTY_EVENT_ID_MIN + 114);
        public const uint EVT_OH_LIGHTS_L_TURNOFF = (THIRD_PARTY_EVENT_ID_MIN + 115);
        public const uint EVT_OH_LIGHTS_R_TURNOFF = (THIRD_PARTY_EVENT_ID_MIN + 116);
        public const uint EVT_OH_LIGHTS_TAXI = (THIRD_PARTY_EVENT_ID_MIN + 117);
        public const uint EVT_OH_LIGHTS_APU_START = (THIRD_PARTY_EVENT_ID_MIN + 118);
        public const uint EVT_OH_LIGHTS_L_ENGINE_START = (THIRD_PARTY_EVENT_ID_MIN + 119);
        public const uint EVT_OH_LIGHTS_IGN_SEL = (THIRD_PARTY_EVENT_ID_MIN + 120);
        public const uint EVT_OH_LIGHTS_R_ENGINE_START = (THIRD_PARTY_EVENT_ID_MIN + 121);
        public const uint EVT_OH_LIGHTS_LOGO = (THIRD_PARTY_EVENT_ID_MIN + 122);
        public const uint EVT_OH_LIGHTS_POS_STROBE = (THIRD_PARTY_EVENT_ID_MIN + 123);
        public const uint EVT_OH_LIGHTS_ANT_COL = (THIRD_PARTY_EVENT_ID_MIN + 124);
        public const uint EVT_OH_LIGHTS_WING = (THIRD_PARTY_EVENT_ID_MIN + 125);
        public const uint EVT_OH_LIGHTS_WHEEL_WELL = (THIRD_PARTY_EVENT_ID_MIN + 126);
        public const uint EVT_OH_LIGHTS_COMPASS = (THIRD_PARTY_EVENT_ID_MIN + 982);

        // Main panel misc
        public const uint EVT_MPM_AUTOBRAKE_SELECTOR = (THIRD_PARTY_EVENT_ID_MIN + 460);

        // Gear panel                                                                                         
        public const uint EVT_GEAR_LEVER = (THIRD_PARTY_EVENT_ID_MIN + 455);
        public const uint EVT_GEAR_LEVER_OFF = (THIRD_PARTY_EVENT_ID_MIN + 4551);
        public const uint EVT_GEAR_LEVER_UNLOCK = (THIRD_PARTY_EVENT_ID_MIN + 4552);

        // Control Stand                                                                                      
        //                                                                                                    
        public const uint EVT_CONTROL_STAND_SPEED_BRAKE_LEVER = (THIRD_PARTY_EVENT_ID_MIN + 679);
        public const uint EVT_CONTROL_STAND_SPEED_BRAKE_LEVER_DOWN = (THIRD_PARTY_EVENT_ID_MIN + 6791);
        public const uint EVT_CONTROL_STAND_SPEED_BRAKE_LEVER_ARM = (THIRD_PARTY_EVENT_ID_MIN + 6792);
        public const uint EVT_CONTROL_STAND_SPEED_BRAKE_LEVER_50PCT = (THIRD_PARTY_EVENT_ID_MIN + 6793);
        public const uint EVT_CONTROL_STAND_SPEED_BRAKE_LEVER_FLT_DET = (THIRD_PARTY_EVENT_ID_MIN + 6794);
        public const uint EVT_CONTROL_STAND_SPEED_BRAKE_LEVER_UP = (THIRD_PARTY_EVENT_ID_MIN + 6795);
        public const uint EVT_CONTROL_STAND_PARK_BRAKE_LEVER = (THIRD_PARTY_EVENT_ID_MIN + 693);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER = (THIRD_PARTY_EVENT_ID_MIN + 714);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER_0 = (THIRD_PARTY_EVENT_ID_MIN + 7141);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER_1 = (THIRD_PARTY_EVENT_ID_MIN + 7142);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER_2 = (THIRD_PARTY_EVENT_ID_MIN + 7143);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER_5 = (THIRD_PARTY_EVENT_ID_MIN + 7144);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER_10 = (THIRD_PARTY_EVENT_ID_MIN + 7145);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER_15 = (THIRD_PARTY_EVENT_ID_MIN + 7146);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER_25 = (THIRD_PARTY_EVENT_ID_MIN + 7147);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER_30 = (THIRD_PARTY_EVENT_ID_MIN + 7148);
        public const uint EVT_CONTROL_STAND_FLAPS_LEVER_40 = (THIRD_PARTY_EVENT_ID_MIN + 7149);
    }
}
