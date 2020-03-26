Option Strict Off
Option Explicit On
Module APS_Define

    ' Initial option
    Public Const INIT_AUTO_CARD_ID As Short = &H0S           '    ' (Bit 0) CardId assigned by system, Input parameter of APS_initial( cardId, "MODE" )
    Public Const INIT_MANUAL_ID As Short = &H1S              '    ' (Bit 0) CardId manual by dip switch, Input parameter of APS_initial( cardId, "MODE" )
    Public Const INIT_PARAM_IGNORE As Short = &H0S           '    ' (Bit 4-5) Load parameter method - ignore, keep current value
    Public Const INIT_PARAM_LOAD_DEFAULT As Short = &H10S    '    ' (Bit 4-5) Load parameter method - load parameter as default value 
    Public Const INIT_PARAM_LOAD_FLASH As Short = &H20S      '    ' (Bit 4-5) Load parameter method - load parameter from flash memory
    Public Const INIT_MNET_INTERRUPT As Short = &H40         ' (Bit 6) Enable MNET interrupt mode. (Support motion interrupt for MotionNet series)

    ' Board parameter define (General
    Public Const PRB_EMG_LOGIC As Short = &H0S ' Board EMG logic
    
    Public Const PRB_DO_LOGIC	As Short	= &H14S	'//DO logic, 0: no invert; 1: invert  
    Public Const PRB_DI_LOGIC	As Short	= &H15S	    '//DI logic, 0: no invert; 1: invert
    
    
    Public Const MHS_GET_SERVO_OFF_INFO As Short	= &H16S 
		Public Const MHS_RESET_SERVO_OFF_INFO	As Short= &H17S
    Public Const MHS_GET_ALL_STATE As Short = &H18S

    Public Const PRB_WDT0_VALUE As Short = &H10S ' Set / Get watch dog limit.
    Public Const PRB_WDT0_COUNTER As Short = &H11S ' Reset Wdt / Get Wdt_Count_Value
    Public Const PRB_WDT0_UNIT As Short = &H12S ' wdt_unit
    Public Const PRB_WDT0_ACTION As Short = &H13S ' wdt_action

    Public Const PRA_HOME_VA As Short = &H16S  'homing approach velocity [PCI-8253/56 only]
    Public Const PRA_HOME_SHIFT As Short = &H17S ' The shift from ORG [PCI-8254/58 only]
    Public Const PRA_HOME_EZA As Short = &H18S ' EZ alignment enable
    Public Const PRA_HOME_VO As Short = &H19 ' Homing leave ORG velocity
    Public Const PRA_HOME_OFFSET As Short = &H1AS ' The escape pulse amounts(Leaving home by position)
    Public Const PRA_HOME_POS As Short = &H1BS 'The position from ORG [PCI-8254/58 only]

    Public Const PRB_TMR0_BASE As Short = &H20S ' Set TMR Value
    Public Const PRB_TMR0_VALUE As Short = &H21S ' Get timer int count value

    Public Const PRA_CURVE As Short = &H20S '// Move curve pattern
    Public Const PRA_SF As Short = &H20S '// Move s-factor
    Public Const PRA_ACC As Short = &H21S '// Move acceleration
    Public Const PRA_DEC As Short = &H22S '// Move deceleration
    Public Const PRA_VS As Short = &H23S '// Move start velocity
    Public Const PRA_VM As Short = &H24S '// Move max velocity
    Public Const PRA_VE As Short = &H25S '// Move end velocity
    Public Const PRA_SACC As Short = &H26S '// S curve acceleration
    Public Const PRA_SDEC As Short = &H27S '// S curve deceleration
    Public Const PRA_ACC_SR As Short = &H28S '// S curve ratio in acceleration( S curve with linear acceleration)
    Public Const PRA_DEC_SR As Short = &H29S '// S curve ratio in deceleration( S curve with linear deceleration)

    Public Const PRA_PRE_EVENT_DIST As Short = &H2AS 'Pre-event distance
    Public Const PRA_POST_EVENT_DIST As Short = &H2BS 'Post-event distance
    
    '//following only for V2...
		Public Const PRA_DIST            As Short = &H30S  '// Move distance
		Public Const PRA_MAX_VELOCITY    As Short = &H31S  '// Maximum velocity
		Public Const PRA_SCUR_PERCENTAGE As Short = &H32S  '// Scurve percentage
		Public Const PRA_BLENDING_MODE   As Short = &H33S  '// Blending mode
		Public Const PRA_STOP_MODE       As Short = &H34S  '// Stop mode
		Public Const PRA_STOP_DELRATE    As Short = &H35S  '// Stop function deceleration rate
		
		Public Const PRA_PT_STOP_ENDO    As Short = &H32S  '// Disable do when point table stopping.
		Public Const PRA_PT_STP_DO_EN    As Short = &H32S  '// Disable do when point table stopping.
		Public Const PRA_PT_STOP_DO      As Short = &H33S  '// Set do value when point table stopping.
		Public Const PRA_PT_STP_DO       As Short = &H33S  '// Set do value when point table stopping.
		Public Const PRA_PWM_OFF         As Short = &H34S  '// Disable specified PWM output when ASTP input signal is active.
		Public Const PRA_DO_OFF          As Short = &H35S  '// Set DO value when ASTP input signal is active.

    Public Const PRB_SYS_TMP_MONITOR As Short = &H30S ' Get system temperature monitor data
    Public Const PRB_CPU_TMP_MONITOR As Short = &H31S ' Get CPU temperature monitor data
    Public Const PRB_AUX_TMP_MONITOR As Short = &H32S ' Get AUX temperature monitor data

    Public Const PRB_UART_MULTIPLIER As Short = &H40S ' Set UART Multiplier

    Public Const PRA_JG_STEP As Short = &H46S '// Jog offset (For step mode)
    Public Const PRA_JG_DELAY As Short = &H47S '// Jog delay (For step mode)
    Public Const PRA_JG_MAP_DI_EN As Short = &H48 '// (I32) Enable Digital input map to jog command signal
    Public Const PRA_JG_P_JOG_DI As Short = &H49S '// (I32) Mapping configuration for positive jog and digital input.
    Public Const PRA_JG_N_JOG_DI As Short = &H4AS '// (I32) Mapping configuration for negative jog and digital input.
    Public Const PRA_JG_JOG_DI As Short = &H4BS '// (I32) Mapping configuration for jog and digital input.

    Public Const PRA_MDN_DELAY As Short = &H50S '// NSTP delay setting
    Public Const PRA_SINP_WDW As Short = &H51S '// Soft INP window setting
    Public Const PRA_SINP_STBL As Short = &H52S '// Soft INP stable cycle
    Public Const PRA_SINP_STBT As Short = &H52S '// Soft INP stable cycle
    Public Const PRA_SERVO_LOGIC As Short = &H53S '//  SERVO logic

    Public Const PRA_GEAR_MASTER As Short = &H60S '// (I32) Select gearing master
    Public Const PRA_GEAR_ENGAGE_RATE As Short = &H61S '// (F64) Gear engage rate
    Public Const PRA_GEAR_RATIO As Short = &H62S '// (F64) Gear ratio
    Public Const PRA_GANTRY_PROTECT_1 As Short = &H63S '// (F64) E-gear gantry mode protection level 1
    Public Const PRA_GANTRY_PROTECT_2 As Short = &H64S '// (F64) E-gear gantry mode protection level 2
    Public Const PRA_MOVE_RATIO As Short = &H88S       '//Move ratio

    'Public Const _PRA_EGEAR As Short = &H84S '8253/6 use this para as unit factor, but 8258 use 0x86. 
    Public Const PRA_ENCODER_DIR As Short = &H85S '(I32) 
    Public Const PRA_POS_UNIT_FACTOR As Short = &H86S '(F64) position unit factor setting

    Public Const PRB_PSR_MODE As Short = &H90S ' Config pulser mode
    Public Const PRB_PSR_EA_LOGIC As Short = &H91S ' Set EA inverted
    Public Const PRB_PSR_EB_LOGIC As Short = &H92S ' Set EB inverted

    ' Board parameter define (For PCI-8253/56
    Public Const PRB_DENOMINATOR As Short = &H80S ' Floating number denominator
    'Public Const  PRB_PSR_MODE          =&H90  ' Config pulser mode
    Public Const PRB_PSR_ENABLE As Short = &H91S ' Enable/disable pulser mode
    Public Const PRB_BOOT_SETTING As Short = &H100S ' Load motion parameter method when DSP boot
    
    Public Const PRB_PWM0_MAP_DO		= &H110S  '// Enable & Map PWM0 to Do channels
		Public Const PRB_PWM1_MAP_DO		= &H111S  '// Enable & Map PWM1 to Do channels
		Public Const PRB_PWM2_MAP_DO		= &H112S  '// Enable & Map PWM2 to Do channels
		Public Const PRB_PWM3_MAP_DO		= &H113S  '// Enable & Map PWM3 to Do channels

    Public Const PRA_D_SAMPLE_TIME As Short = &H12CS '(I32) Derivative Sample Time

    Public Const PRA_BIQUAD0_A1 As Short = &H132S '(F64) Biquad filter0 coefficient A1
    Public Const PRA_BIQUAD0_A2 As Short = &H133S '(F64) Biquad filter0 coefficient A2
    Public Const PRA_BIQUAD0_B0 As Short = &H134S '(F64) Biquad filter0 coefficient B0
    Public Const PRA_BIQUAD0_B1 As Short = &H135S '(F64) Biquad filter0 coefficient B1
    Public Const PRA_BIQUAD0_B2 As Short = &H136S '(F64) Biquad filter0 coefficient B2
    Public Const PRA_BIQUAD0_DIV As Short = &H137S '(F64) Biquad filter0 divider
    Public Const PRA_BIQUAD1_A1 As Short = &H138S '(F64) Biquad filter1 coefficient A1
    Public Const PRA_BIQUAD1_A2 As Short = &H139S '(F64) Biquad filter1 coefficient A2
    Public Const PRA_BIQUAD1_B0 As Short = &H13AS '(F64) Biquad filter1 coefficient B0
    Public Const PRA_BIQUAD1_B1 As Short = &H13BS '(F64) Biquad filter1 coefficient B1
    Public Const PRA_BIQUAD1_B2 As Short = &H13CS '(F64) Biquad filter1 coefficient B2
    Public Const PRA_BIQUAD1_DIV As Short = &H13DS '(F64) Biquad filter1 divider
    Public Const PRA_FRIC_GAIN		  As Short = &H13ES '// (F64) Friction voltage compensation

    ' Board parameter define (For PCI-8392 SSCNET
    Public Const PRB_SSC_APPLICATION As Integer = &H10000 ' Reserved
    Public Const PRB_SSC_CYCLE_TIME As Integer = &H10000 ' SSCNET cycle time selection(vaild befor start sscnet
    Public Const PRB_PARA_INIT_OPT As Short = &H20S ' Initial boot mode.

    ' Board parameter define (For DPAC
    Public Const PRB_DPAC_DISPLAY_MODE As Integer = &H10001 'DPAC Display mode
    Public Const PRB_DPAC_DI_MODE As Integer = &H10002 'Set DI pin modes

    Public Const PRB_DPAC_THERMAL_MONITOR_NO As Integer = &H20001 'DPAC TEST
    Public Const PRB_DPAC_THERMAL_MONITOR_VALUE As Integer = &H20002 'DPAC TEST

    ' Axis parameter define (General
    Public Const PRA_EL_LOGIC As Short = &H0S ' EL logic
    Public Const PRA_ORG_LOGIC As Short = &H1S ' ORG logic
    Public Const PRA_EL_MODE As Short = &H2S ' EL stop mode
    Public Const PRA_MDM_CONDI As Short = &H3S ' Motion done condition
    Public Const PRA_EL_EXCHANGE As Short = &H4S ' PEL, MEL exchange enable

    Public Const PRA_ALM_LOGIC As Short = &H4S ' ALM logic [PCI-8253/56 only]
    Public Const PRA_ZSP_LOGIC As Short = &H5S ' ZSP logic [PCI-8253/56 only]
    Public Const PRA_EZ_LOGIC As Short = &H6S ' EZ logic  [PCI-8253/56 only]
    Public Const PRA_STP_DEC As Short = &H7S ' Stop deceleration
    Public Const PRA_SPEL_EN As Short = &H8S ' SPEL Enable
    Public Const PRA_SMEL_EN As Short = &H9S ' SMEL Enable
    Public Const PRA_EFB_POS0 As Short = &HAS ' EFB position 0
    Public Const PRA_SPEL_POS As Short = &HAS ' EFB position 0
    Public Const PRA_EFB_POS1 As Short = &HBS ' EFB position 1
    Public Const PRA_SMEL_POS As Short = &HBS ' EFB position 1
    Public Const PRA_EFB_CONDI0 As Short = &HCS ' EFB position 0 condition
    Public Const PRA_EFB_CONDI1 As Short = &HDS ' EFB position 1 condition
    Public Const PRA_EFB_SRC0 As Short = &HES ' EFB position 0 source
    Public Const PRA_EFB_SRC1 As Short = &HFS ' EFB position 1 source
    Public Const PRA_HOME_MODE As Short = &H10S ' home mode
    Public Const PRA_HOME_DIR As Short = &H11S ' homing direction
    Public Const PRA_HOME_CURVE As Short = &H12S ' homing curve parten(T or s curve
    Public Const PRA_HOME_ACC As Short = &H13S ' Acceleration deceleration rate
    Public Const PRA_HOME_VS As Short = &H14S ' homing start velocity
    Public Const PRA_HOME_VM As Short = &H15S ' homing max velocity
    'Public Const PRA_HOME_VA As Short = &H16S ' homing approach velocity     [PCI-8253/56 only]
    'Public Const PRA_HOME_EZA As Short = &H18S ' EZ alignment enable
    'Public Const PRA_HOME_VO As Short = &H19S ' Homing leave ORG velocity
    'Public Const PRA_HOME_OFFSET As Short = &H1AS ' The escape pulse amounts(Leaving home by position

    Public Const PRA_JG_MODE As Short = &H40S ' Jog mode
    Public Const PRA_JG_DIR As Short = &H41S ' Jog move direction
    Public Const PRA_JG_CURVE As Short = &H42S ' Jog curve parten(T or s curve
    Public Const PRA_JG_SF As Short = &H42S ' Jog curve parten(T or s curve
    Public Const PRA_JG_ACC As Short = &H43S ' Jog move acceleration
    Public Const PRA_JG_DEC As Short = &H44S ' Jog move deceleration
    Public Const PRA_JG_VM As Short = &H45S ' Jog move max velocity

    ' Axis parameter define (For PCI-8253/56
    Public Const PRA_PLS_IPT_MODE As Short = &H80S ' Pulse input mode setting
    Public Const PRA_PLS_OPT_MODE As Short = &H81S ' Pulse output mode setting
    Public Const PRA_MAX_E_LIMIT As Short = &H82S ' Maximum encoder count limit
    Public Const PRA_ENC_FILTER As Short = &H83S ' Encoder filter
    Public Const PRA_ENCODER_FILTER As Short = &H83S ' Encoder filter
    Public Const PRA_EGEAR As Short = &H84S ' E-Gear ratio

    Public Const PRA_KP_GAIN As Short = &H90S ' PID controller Kp gain
    Public Const PRA_KI_GAIN As Short = &H91S ' PID controller Ki gain
    Public Const PRA_KD_GAIN As Short = &H92S ' PID controller Kd gain
    Public Const PRA_KFF_GAIN As Short = &H93S ' Feed forward Kff gain
    Public Const PRA_KVFF_GAIN As Short = &H93S ' Feed forward Kff gain
    
    Public Const PRA_KVGTY_GAIN As Short = &H94S ' Gantry controller Kvgty gain
    Public Const PRA_KPGTY_GAIN As Short = &H95S ' Gantry controller Kpgty gain
    Public Const PRA_IKP_GAIN As Short = &H96S ' PID controller Kp gain in torque mode
    Public Const PRA_IKI_GAIN As Short = &H97S ' PID controller Ki gain in torque mode
    Public Const PRA_IKD_GAIN As Short = &H98S ' PID controller Kd gain in torque mode
    Public Const PRA_IKFF_GAIN As Short = &H99S ' Feed forward Kff gain in torque mode
    Public Const PRA_KAFF_GAIN As Short = &H9AS  ' Acceleration feedforward Kaff gain
    
    Public Const PRA_KP_SHIFT As Short = &H9BS           '//Proportional control result shift
    Public Const PRA_KI_SHIFT As Short = &H9CS            '//Integral control result shift
    Public Const PRA_KD_SHIFT As Short = &H9DS           '// Derivative control result shift
    Public Const PRA_KVFF_SHIFT As Short = &H9ES          '//Velocity feed-forward control result shift
    Public Const PRA_KAFF_SHIFT As Short = &H9FS          '//Acceleration feed-forward control result shift
    Public Const PRA_PID_SHIFT  As Short = &HA0S          '//PID control result shift
    
    
    
    
    
    '//following only for V2...
		Public Const PRA_VOLTAGE_MAX     As Short = &H9BS  ' Maximum output limit
		Public Const PRA_VOLTAGE_MIN     As Short = &H9CS  ' Minimum output limit

    Public Const PRA_M_INTERFACE As Short = &H100S ' Motion interface

    Public Const PRA_M_VOL_RANGE As Short = &H110S ' Motor voltage input range
    Public Const PRA_M_MAX_SPEED As Short = &H111S ' Motor maximum speed
    Public Const PRA_M_ENC_RES As Short = &H112S ' Motor encoder resolution

    Public Const PRA_V_OFFSET As Short = &H120S ' Voltage offset
    Public Const PRA_SERVO_V_BIAS As Short = &H120S ' Voltage offset
    Public Const PRA_DZ_LOW As Short = &H121S ' Dead zone low side
    Public Const PRA_DZ_UP As Short = &H122S ' Dead zone up side
    Public Const PRA_SAT_LIMIT As Short = &H123S ' Voltage saturation output limit
    Public Const PRA_SERVO_V_LIMIT  As Short = &H123S ' Voltage saturation output limit
    Public Const PRA_ERR_C_LEVEL As Short = &H124S ' Error counter check level
    Public Const PRA_ERR_POS_LEVEL As Short = &H124S ' Error counter check level
    Public Const PRA_V_INVERSE As Short = &H125S ' Output voltage inverse
    Public Const PRA_SERVO_V_INVERSE As Short = &H125S ' Output voltage inverse
    Public Const PRA_PSR_LINK As Short = &H130S ' Connect pulser number
    Public Const PRA_PSR_RATIO As Short = &H131S ' Set pulser ratio
    Public Const PRA_DA_TYPE As Short = &H140S ' DAC output type
    Public Const PRA_CONTROL_MODE As Short = &H141S ' Closed loop control mode
    Public Const PRA_BKL_DIST As Short = &H129S 'Backlash distance
    Public Const PRA_BKL_CNSP As Short = &H12AS 'Backlash consumption
    Public Const PRA_INTEGRAL_LIMIT As Short = &H12BS '(I32) Integral limit

    ' Axis parameter define (For PCI-8154/58
    ' Input/Output Mode
    Public Const PRA_PLS_IPT_LOGIC As Short = &H200S 'Reverse pulse input counting
    Public Const PRA_FEEDBACK_SRC As Short = &H201S 'Select feedback conter

    'IO Config
    Public Const PRA_ALM_MODE As Short = &H210S 'ALM Mode
    Public Const PRA_INP_LOGIC As Short = &H211S 'INP Logic
    Public Const PRA_SD_EN As Short = &H212S 'SD Enable -- Bit 8
    Public Const PRA_SD_MODE As Short = &H213S 'SD Mode
    Public Const PRA_SD_LOGIC As Short = &H214S 'SD Logic
    Public Const PRA_SD_LATCH As Short = &H215S 'SD Latch
    Public Const PRA_ERC_MODE As Short = &H216S 'ERC Mode
    Public Const PRA_ERC_LOGIC As Short = &H217S 'ERC logic
    Public Const PRA_ERC_LEN As Short = &H218S 'ERC pulse width
    Public Const PRA_CLR_MODE As Short = &H219S 'CLR Mode
    Public Const PRA_CLR_TARGET As Short = &H21AS 'CLR Target counter
    Public Const PRA_PIN_FLT As Short = &H21BS 'EA/EB Filter Enable
    Public Const PRA_INP_MODE As Short = &H21CS 'INP Mode
    Public Const PRA_LTC_LOGIC As Short = &H21DS 'LTC LOGIC
    Public Const PRA_SOFT_PLIMIT As Short = &H21ES 'SOFT PLIMIT
    Public Const PRA_SOFT_MLIMIT As Short = &H21FS 'SOFT MLIMIT
    Public Const PRA_SOFT_LIMIT_EN As Short = &H220S 'SOFT ENABLE
    Public Const PRA_BACKLASH_PULSE As Short = &H221S 'BACKLASH PULSE
    Public Const PRA_BACKLASH_MODE As Short = &H222S 'BACKLASH MODE
    Public Const PRA_LTC_SRC As Short = &H223S 'LTC Source
    Public Const PRA_LTC_DEST As Short = &H224S 'LTC Destination
    Public Const PRA_LTC_DATA As Short = &H225S 'Get LTC DATA
    Public Const PRA_GCMP_EN As Short = &H226S ' CMP Enable
    Public Const PRA_GCMP_POS As Short = &H227S ' Get CMP position
    Public Const PRA_GCMP_SRC As Short = &H228S ' CMP source
    Public Const PRA_GCMP_ACTION As Short = &H229S ' CMP Action
    Public Const PRA_GCMP_STS As Short = &H22AS ' CMP Status
    Public Const PRA_VIBSUP_RT       As Short = &H22BS '// Vibration Reverse Time
		Public Const PRA_VIBSUP_FT       As Short = &H22CS '// Vibration Forward Time
		Public Const PRA_LTC_DATA_SPD      As Short = &H22DS '// Choose latch data for current speed or error position

    Public Const PRA_GPDO_SEL As Short = &H230S 'Select DO/CMP Output mode
    Public Const PRA_GPDI_SEL As Short = &H231S 'Select DO/CMP Output mode
    Public Const PRA_GPDI_LOGIC As Short = &H232S 'Set gpio input logic
    Public Const PRA_RDY_LOGIC As Short = &H233S 'RDY logic

    'Fixed Speed
    Public Const PRA_SPD_LIMIT As Short = &H240S ' Set Fixed Speed
    Public Const PRA_MAX_ACCDEC As Short = &H241S ' Get max acceleration by fixed speed
    Public Const PRA_MIN_ACCDEC As Short = &H242S ' Get min acceleration by fixed speed
    Public Const PRA_ENABLE_SPD As Short = &H243S ' Disable/Enable Fixed Speed only for HSL-4XMO.

    'Continuous Move
    Public Const PRA_CONTI_MODE As Short = &H250S ' Continuous Mode
    Public Const PRA_CONTI_BUFF As Short = &H251S ' Continuous Buffer
    
    'Simultaneous Move
    Public Const PRA_SYNC_STOP_MODE      As Short = &H260S '// Sync Mode

    ' PCI-8144 axis parameter define
    Public Const PRA_CMD_CNT_EN As Integer = &H10000
    Public Const PRA_MIO_SEN As Integer = &H10001
    Public Const PRA_START_STA As Integer = &H10002
    Public Const PRA_SPEED_CHN As Integer = &H10003
    Public Const PRA_ORG_STP As Short = &H1AS

    ' Axis parameter define (For PCI-8392 SSCNET
    Public Const PRA_SSC_SERVO_PARAM_SRC As Integer = &H10000 'Servo parameter source
    Public Const PRA_SSC_SERVO_ABS_POS_OPT As Integer = &H10001 'Absolute position system option
    Public Const PRA_SSC_SERVO_ABS_CYC_CNT As Integer = &H10002 'Absolute cycle counter of servo driver
    Public Const PRA_SSC_SERVO_ABS_RES_CNT As Integer = &H10003 'Absolute resolution counter of servo driver
    Public Const PRA_SSC_TORQUE_LIMIT_P As Integer = &H10004 'Torque limit positive (0.1%
    Public Const PRA_SSC_TORQUE_LIMIT_N As Integer = &H10005 'Torque limit negative (0.1%
    Public Const PRA_SSC_TORQUE_CTRL As Integer = &H10006 'Torque control
    Public Const PRA_SSC_RESOLUTION As Integer = &H10007 'resolution (E-gear
    Public Const PRA_SSC_GMR As Integer = &H10008 'resolution (New E-gear
    Public Const PRA_SSC_GDR As Integer = &H10009 'resolution (New E-gear

    ' Sampling parameter define
    Public Const SAMP_PA_RATE As Short = &H0S 'Sampling rate
    Public Const SAMP_PA_EDGE As Short = &H2S 'Edge select
    Public Const SAMP_PA_LEVEL As Short = &H3S 'Level select
    Public Const SAMP_PA_TRIGCH As Short = &H5S 'Select trigger channel
    
    '//following only for V2
		Public Const SAMP_PA_SEL         As Short = &H6S

    Public Const SAMP_PA_SRC_CH0 As Short = &H10S 'Sample source of channel 0
    Public Const SAMP_PA_SRC_CH1 As Short = &H11S 'Sample source of channel 1
    Public Const SAMP_PA_SRC_CH2 As Short = &H12S 'Sample source of channel 2
    Public Const SAMP_PA_SRC_CH3 As Short = &H13S 'Sample source of channel 3

    ' Sampling source
    Public Const SAMP_AXIS_MASK As Short = &HF00S
    Public Const SAMP_PARAM_MASK As Short = &HFFS
    Public Const SAMP_COM_POS As Short = &H0S 'command position
    Public Const SAMP_FBK_POS As Short = &H1S 'feedback position
    Public Const SAMP_CMD_VEL As Short = &H2S 'command velocity
    Public Const SAMP_FBK_VEL As Short = &H3S 'feedback velocity
    Public Const SAMP_MIO As Short = &H4S 'motion IO
    Public Const SAMP_MSTS As Short = &H5S 'motion status
    Public Const SAMP_MSTS_ACC As Short = &H6S 'motion status acc
    Public Const SAMP_MSTS_MV As Short = &H7S 'motion status at max velocity
    Public Const SAMP_MSTS_DEC As Short = &H8S 'motion status at dec
    Public Const SAMP_MSTS_CSTP As Short = &H9S 'motion status CSTP
    Public Const SAMP_MSTS_NSTP As Short = &HAS 'motion status NSTP
    Public Const SAMP_MSTS_MDN As Short = &HAS 'motion status NSTP
    Public Const SAMP_MIO_INP As Short = &HBS 'motion status INP
    Public Const SAMP_MIO_ZERO As Short = &HCS 'motion status ZERO
    Public Const SAMP_MIO_ORG As Short = &HDS 'motion status OGR

    Public Const SAMP_SSC_MON_0 As Short = &H10S ' SSCNET servo monitor ch0
    Public Const SAMP_SSC_MON_1 As Short = &H11S ' SSCNET servo monitor ch1
    Public Const SAMP_SSC_MON_2 As Short = &H12S ' SSCNET servo monitor ch2
    Public Const SAMP_SSC_MON_3 As Short = &H13S ' SSCNET servo monitor ch3
    
    '//Only for PCI-8254/8, AMP-204/8C
		Public Const SAMP_COM_POS_F64						As Short= &H10S '// Command position
		Public Const SAMP_FBK_POS_F64						As Short= &H11S '//Feedback position
		Public Const SAMP_CMD_VEL_F64						As Short= &H12S '// Command velocity
		Public Const SAMP_FBK_VEL_F64						As Short= &H13S '// Feedback velocity
		Public Const SAMP_CONTROL_VOL_F64				As Short= &H14S '// Control command voltage
		Public Const SAMP_ERR_POS_F64						As Short= &H15S '// Error position
		Public Const SAMP_PWM_FREQUENCY_F64		  As Short= &H18S '// PWM frequency (Hz)
		Public Const SAMP_PWM_DUTY_CYCLE_F64		As Short= &H19S '// PWM duty cycle (%)
		Public Const SAMP_PWM_WIDTH_F64					As Short= &H1AS '// PWM width (ns)
		Public Const SAMP_VAO_COMP_VEL_F64			As Short= &H1BS '// Composed velocity for Laser power control (pps)
		Public Const SAMP_PTBUFF_COMP_VEL_F64		As Short= &H1CS '// Composed velocity of point table
		Public Const SAMP_PTBUFF_COMP_ACC_F64		As Short= &H1DS '// Composed acceleration of point table

    Public Const SAMP_CONTROL_VOL As Short = &H20S '
    Public Const SAMP_GTY_DEVIATION As Short = &H21S
    Public Const SAMP_ENCODER_RAW As Short = &H22S
    Public Const SAMP_ERROR_COUNTER As Short = &H23S
    Public Const SAMP_ERROR_POS					As Short = &H23S '//Error position [PCI-8254/58]
		Public Const SAMP_PTBUFF_RUN_INDEX		As Short = &H24S '//Point table running index

    'FieldBus parameter define
    Public Const PRF_COMMUNICATION_TYPE As Short = &H0S ' FiledBus Communication Type(Full/half duplex
    Public Const PRF_TRANSFER_RATE As Short = &H1S ' FiledBus Transfer Rate
    Public Const PRF_HUB_NUMBER As Short = &H2S ' FiledBus Hub Number
    Public Const PRF_INITIAL_TYPE As Short = &H3S ' FiledBus Initial Type(Clear/Reserve Do area
    Public Const PRF_CHKERRCNT_LAYER         As Short = &H04S '// Set the check error count layer.

    'Gantry parameter number define [Only for PCI-8392, PCI-8253/56]
    Public Const GANTRY_MODE As Short = &H0S
    Public Const GENTRY_DEVIATION As Short = &H1S
    Public Const GENTRY_DEVIATION_STP As Short = &H2S
    
    ' Filter parameter number define [Only for PCI-8253/56]
		Public Const FTR_TYPE_ST0                    As Short = &H00S  '// Station 0 filter type
		Public Const FTR_FC_ST0                      As Short = &H01S  '// Station 0 filter cutoff frequency
		Public Const FTR_BW_ST0                      As Short = &H02S  '// Station 0 filter bandwidth
		Public Const FTR_ENABLE_ST0                  As Short = &H03S  '// Station 0 filter enable/disable
		Public Const FTR_TYPE_ST1                    As Short = &H10S  '// Station 1 filter type
		Public Const FTR_FC_ST1                      As Short = &H11S  '// Station 1 filter cutoff frequency
    Public Const FTR_BW_ST1 As Short = &H12S  '// Station 1 filter bandwidth
		Public Const FTR_ENABLE_ST1                  As Short = &H13S  '// Station 1 filter enable/disable


    ' Filter parameter number define [Only for PCI-8253/56]
    Public Const FTR_TYPE As Short = &H0S ' Filter type
    Public Const FTR_FC As Short = &H1S ' Filter cutoff frequency
    Public Const FTR_BW As Short = &H2S ' Filter bandwidth
    Public Const FTR_ENABLE As Short = &H3S ' Filter enable/disable

    ' Device name define
    Public Const DEVICE_NAME_NULL As Short = &HFFFFS
    Public Const DEVICE_NAME_PCI_8392 As Short = 0
    Public Const DEVICE_NAME_PCI_825X As Short = 1
    Public Const DEVICE_NAME_PCI_8154 As Short = 2
    Public Const DEVICE_NAME_PCI_785X As Short = 3
    Public Const DEVICE_NAME_PCI_8158 As Short = 4
    Public Const DEVICE_NAME_PCI_7856 As Short = 5
    Public Const DEVICE_NAME_ISA_DPAC1000 As Short = 6
    Public Const DEVICE_NAME_ISA_DPAC3000 As Short = 7
    Public Const DEVICE_NAME_PCI_8144 As Short = 8
    Public Const DEVICE_NAME_PCI_825458 As Short = 9
    Public Const DEVICE_NAME_PCI_8102 As Short = 10
    Public Const DEVICE_NAME_PCI_V8258 As Short = 11
    Public Const DEVICE_NAME_PCI_V8254 As Short = 12
    Public Const DEVICE_NAME_PCI_8158A As Short = 13
    Public Const DEVICE_NAME_AMP_20408C As Short = 14

    '''''''''''''''''''''''/
    '   HSL Slave module definition
    '''''''''''''''''''''''/
    Public Const SLAVE_NAME_UNKNOWN As Short = &H0S
    Public Const SLAVE_NAME_HSL_DI32 As Short = &H100S
    Public Const SLAVE_NAME_HSL_DO32 As Short = &H101S
    Public Const SLAVE_NAME_HSL_DI16DO16 As Short = &H102S
    Public Const SLAVE_NAME_HSL_AO4 As Short = &H103S
    Public Const SLAVE_NAME_HSL_AI16AO2_VV As Short = &H104S
    Public Const SLAVE_NAME_HSL_AI16AO2_AV As Short = &H105S
    Public Const SLAVE_NAME_HSL_DI16UL As Short = &H106S
    Public Const SLAVE_NAME_HSL_DI16RO8 As Short = &H107S
    Public Const SLAVE_NAME_HSL_4XMO As Short = &H108S
    Public Const SLAVE_NAME_HSL_DI16_UCT     As Short = &H109S
		Public Const SLAVE_NAME_HSL_DO16_UCT     As Short = &H10AS
		Public Const SLAVE_NAME_HSL_DI8DO8       As Short = &H10BS

    '''''''''''''''''''''''/
    '   MNET Slave module definition
    '''''''''''''''''''''''/
    Public Const SLAVE_NAME_MNET_1XMO As Short = &H200S
    Public Const SLAVE_NAME_MNET_4XMO As Short = &H201S
    Public Const SLAVE_NAME_MNET_4XMO_C As Short = &H202S
    
    '//Trigger parameter number define. [Only for DB-8150]
			Public Const TG_PWM0_PULSE_WIDTH    As Short = &H00S
			Public Const TG_PWM1_PULSE_WIDTH    As Short = &H01S	
			Public Const TG_PWM0_MODE           As Short = &H02S
			Public Const TG_PWM1_MODE           As Short = &H03S			
			Public Const TG_TIMER0_INTERVAL     As Short = &H04S
			Public Const TG_TIMER1_INTERVAL     As Short = &H05S			
			Public Const TG_ENC0_CNT_DIR        As Short = &H06S
			Public Const TG_ENC1_CNT_DIR       As Short = &H07S			
			Public Const TG_IPT0_MODE           As Short = &H08S
			Public Const TG_IPT1_MODE           As Short = &H09S			
			Public Const TG_EZ0_CLEAR_EN        As Short = &H0AS
			Public Const TG_EZ1_CLEAR_EN        As Short = &H0BS			
			Public Const TG_EZ0_CLEAR_LOGIC     As Short = &H0CS
			Public Const TG_EZ1_CLEAR_LOGIC     As Short = &H0DS			
			Public Const TG_CNT0_SOURCE         As Short = &H0ES
			Public Const TG_CNT1_SOURCE         As Short = &H0FS			
			Public Const TG_FTR0_EN		       As Short = &H10S
			Public Const TG_FTR1_EN		       As Short = &H11S			
			Public Const TG_DI_LATCH0_EN        As Short = &H12S
			Public Const TG_DI_LATCH1_EN        As Short = &H13S			
			Public Const TG_DI_LATCH0_EDGE      As Short = &H14S
			Public Const TG_DI_LATCH1_EDGE      As Short = &H15S			
			Public Const TG_DI_LATCH0_VALUE     As Short = &H16S
			Public Const TG_DI_LATCH1_VALUE     As Short = &H17S			
			Public Const TG_TRGOUT_MAP          As Short = &H18S
			Public Const TG_TRGOUT_LOGIC        As Short = &H19S			
			Public Const TG_FIFO_LEVEL          As Short = &H1AS			
			Public Const TG_PWM0_SOURCE         As Short = &H1BS
			Public Const TG_PWM1_SOURCE         As Short = &H1CS


    'Trigger parameter number define. [Only for PCI-8253/56]
    Public Const TG_LCMP0_SRC As Short = &H0S
    Public Const TG_LCMP1_SRC As Short = &H1S
    Public Const TG_TCMP0_SRC As Short = &H2S
    Public Const TG_TCMP1_SRC As Short = &H3S

    Public Const TG_LCMP0_EN As Short = &H4S
    Public Const TG_LCMP1_EN As Short = &H5S
    Public Const TG_TCMP0_EN As Short = &H6S
    Public Const TG_TCMP1_EN As Short = &H7S

    Public Const TG_TRG0_SRC As Short = &H10S
    Public Const TG_TRG1_SRC As Short = &H11S
    Public Const TG_TRG2_SRC As Short = &H12S
    Public Const TG_TRG3_SRC As Short = &H13S

    Public Const TG_TRG0_PWD As Short = &H14S
    Public Const TG_TRG1_PWD As Short = &H15S
    Public Const TG_TRG2_PWD As Short = &H16S
    Public Const TG_TRG3_PWD As Short = &H17S

    Public Const TG_TRG0_CFG As Short = &H18S
    Public Const TG_TRG1_CFG As Short = &H19S
    Public Const TG_TRG2_CFG As Short = &H1AS
    Public Const TG_TRG3_CFG As Short = &H1BS

    Public Const TMR_ITV As Short = &H20S
    Public Const TMR_EN As Short = &H21S

    'Trigger parameter number define. [Only for MNET-4XMO-C]
    Public Const TG_CMP0_SRC As Short = &H0S
    Public Const TG_CMP1_SRC As Short = &H1S
    Public Const TG_CMP2_SRC As Short = &H2S
    Public Const TG_CMP3_SRC As Short = &H3S
    Public Const TG_CMP0_EN As Short = &H4S
    Public Const TG_CMP1_EN As Short = &H5S
    Public Const TG_CMP2_EN As Short = &H6S
    Public Const TG_CMP3_EN As Short = &H7S
    Public Const TG_CMP0_TYPE As Short = &H8S
    Public Const TG_CMP1_TYPE As Short = &H9S
    Public Const TG_CMP2_TYPE As Short = &HAS
    Public Const TG_CMP3_TYPE As Short = &HBS
    Public Const TG_CMPH_EN As Short = &HCS
    Public Const TG_CMPH_DIR_EN As Short = &HDS
    Public Const TG_CMPH_DIR As Short = &HES

    'Public Const  TG_TRG0_SRC                   =&H10
    'Public Const  TG_TRG1_SRC                   =&H11
    'Public Const  TG_TRG2_SRC                   =&H12
    'Public Const  TG_TRG3_SRC                   =&H13

    'Public Const  TG_TRG0_PWD                   =&H14
    'Public Const  TG_TRG1_PWD                   =&H15
    'Public Const  TG_TRG2_PWD                   =&H16
    'Public Const  TG_TRG3_PWD                   =&H17

    'Public Const  TG_TRG0_CFG                   =&H18
    'Public Const  TG_TRG1_CFG                   =&H19
    'Public Const  TG_TRG2_CFG                   =&H1A
    'Public Const  TG_TRG3_CFG                   =&H1B

    Public Const TG_ENCH_CFG As Short = &H20S

    Public Const TG_TRG0_CMP_DIR As Short = &H21S
    Public Const TG_TRG1_CMP_DIR As Short = &H22S
    Public Const TG_TRG2_CMP_DIR As Short = &H23S
    Public Const TG_TRG3_CMP_DIR As Short = &H24S

    ' Motion IO status bit number define.
    Public Const MIO_ALM As Short = 0 ' Servo alarm.
    Public Const MIO_PEL As Short = 1 ' Positive end limit.
    Public Const MIO_MEL As Short = 2 ' Negative end limit.
    Public Const MIO_ORG As Short = 3 ' ORG =Home
    Public Const MIO_EMG As Short = 4 ' Emergency stop
    Public Const MIO_EZ As Short = 5 ' EZ.
    Public Const MIO_INP As Short = 6 ' In position.
    Public Const MIO_SVON As Short = 7 ' Servo on signal.
    Public Const MIO_RDY As Short = 8 ' Ready.
    Public Const MIO_WARN As Short = 9 ' Warning.
    Public Const MIO_ZSP As Short = 10 ' Zero speed.
    Public Const MIO_SPEL As Short = 11 ' Soft positive end limit.
    Public Const MIO_SMEL As Short = 12 ' Soft negative end limit.
    Public Const MIO_TLC As Short = 13 ' Torque is limited by torque limit value.
    Public Const MIO_ABSL As Short = 14 ' Absolute position lost.
    Public Const MIO_STA As Short = 15 ' External start signal.
    Public Const MIO_PSD As Short = 16 ' Positive slow down signal
    Public Const MIO_MSD As Short = 17 ' Negative slow down signal

    ' Motion status bit number define.
    Public Const MTS_CSTP As Short = 0 ' Command stop signal.
    Public Const MTS_VM As Short = 1 ' At maximum velocity.
    Public Const MTS_ACC As Short = 2 ' In acceleration.
    Public Const MTS_DEC As Short = 3 ' In deceleration.
    Public Const MTS_DIR As Short = 4 ' LastMoving direction.
    Public Const MTS_NSTP As Short = 5 ' Normal stop(Motion done.
    Public Const MTS_HMV As Short = 6 ' In home operation.
    Public Const MTS_SMV As Short = 7 ' Single axis move relative, absolute, velocity move.
    Public Const MTS_LIP As Short = 8 ' Linear interpolation.
    Public Const MTS_CIP As Short = 9 ' Circular interpolation.
    Public Const MTS_VS As Short = 10 ' At start velocity.
    Public Const MTS_PMV As Short = 11 ' Point table move.
    Public Const MTS_PDW As Short = 12 ' Point table dwell move.
    Public Const MTS_PPS As Short = 13 ' Point table pause state.
    Public Const MTS_SLV As Short = 14 ' Slave axis move.
    Public Const MTS_JOG As Short = 15 ' Jog move.
    Public Const MTS_ASTP As Short = 16 ' Abnormal stop.
    Public Const MTS_SVONS As Short = 17 ' Servo off stopped.
    Public Const MTS_EMGS As Short = 18 ' EMG / SEMG stopped.
    Public Const MTS_ALMS As Short = 19 ' Alarm stop.
    Public Const MTS_WANS As Short = 20 ' Warning stopped.
    Public Const MTS_PELS As Short = 21 ' PEL stopped.
    Public Const MTS_MELS As Short = 22 ' MEL stopped.
    Public Const MTS_ECES As Short = 23 ' Error counter check level reaches and stopped.
    Public Const MTS_SPELS As Short = 24 ' Soft PEL stopped.
    Public Const MTS_SMELS As Short = 25 ' Soft MEL stopped.
    Public Const MTS_STPOA As Short = 26 ' Stop by others axes.
    Public Const MTS_GDCES As Short = 27 ' Gantry deviation error level reaches and stopped.
    Public Const MTS_GTM As Short = 28 ' Gantry mode turn on.
    Public Const MTS_PAPB As Short = 29 ' Pulsar mode turn on.
    
    'Following definition for PCI-8254/8
		Public Const MTS_MDN       As Short =5         '// Motion done. 0: In motion, 1: Motion done ( It could be abnormal stop)
		Public Const MTS_WAIT      As Short =10        '// Axis is in waiting state. ( Wait move trigger )
		Public Const MTS_PTB        As Short= 11       ' // Axis is in point buffer moving. ( When this bit on, MDN and ASTP will be cleared )
		Public Const MTS_BLD		   As Short =17        '// Axis (Axes) in blending moving
		Public Const MTS_PRED       As Short=18        '// Pre-distance event, 1: event arrived. The event will be clear when axis start moving 
		Public Const MTS_POSTD    As Short =19        '// Post-distance event. 1: event arrived. The event will be clear when axis start moving
		Public Const MTS_GER        As Short= 28       ' // 1: In geared ( This axis as slave axis and it follow a master specified in axis parameter. )

    ' Motion IO status bit value define.
    Public Const MIO_ALM_V As Short = &H1S ' Servo alarm.
    Public Const MIO_PEL_V As Short = &H2S ' Positive end limit.
    Public Const MIO_MEL_V As Short = &H4S ' Negative end limit.
    Public Const MIO_ORG_V As Short = &H8S ' ORG =Home.
    Public Const MIO_EMG_V As Short = &H10S ' Emergency stop.
    Public Const MIO_EZ_V As Short = &H20S ' EZ.
    Public Const MIO_INP_V As Short = &H40S ' In position.
    Public Const MIO_SVON_V As Short = &H80S ' Servo on signal.
    Public Const MIO_RDY_V As Short = &H100S ' Ready.
    Public Const MIO_WARN_V As Short = &H200S ' Warning.
    Public Const MIO_ZSP_V As Short = &H400S ' Zero speed.
    Public Const MIO_SPEL_V As Short = &H800S ' Soft positive end limit.
    Public Const MIO_SMEL_V As Short = &H1000S ' Soft negative end limit.
    Public Const MIO_TLC_V As Short = &H2000S ' Torque is limited by torque limit value.
    Public Const MIO_ABSL_V As Short = &H4000S ' Absolute position lost.
    Public Const MIO_STA_V As Short = &H8000S ' External start signal.
    Public Const MIO_PSD_V As Integer = &H10000 ' Positive slow down signal.
    Public Const MIO_MSD_V As Integer = &H20000 ' Negative slow down signal.

    ' Motion status bit value define.
    Public Const MTS_CSTP_V As Short = &H1S ' Command stop signal.
    Public Const MTS_VM_V As Short = &H2S ' At maximum velocity.
    Public Const MTS_ACC_V As Short = &H4S ' In acceleration.
    Public Const MTS_DEC_V As Short = &H8S ' In deceleration.
    Public Const MTS_DIR_V As Short = &H10S ' LastMoving direction.
    Public Const MTS_NSTP_V As Short = &H20S ' Normal stop Motion done.
    Public Const MTS_HMV_V As Short = &H40S ' In home operation.
    Public Const MTS_SMV_V As Short = &H80S ' Single axis move( relative, absolute, velocity move.
    Public Const MTS_LIP_V As Short = &H100S ' Linear interpolation.
    Public Const MTS_CIP_V As Short = &H200S ' Circular interpolation.
    Public Const MTS_VS_V As Short = &H400S ' At start velocity.
    Public Const MTS_PMV_V As Short = &H800S ' Point table move.
    Public Const MTS_PDW_V As Short = &H1000S ' Point table dwell move.
    Public Const MTS_PPS_V As Short = &H2000S ' Point table pause state.
    Public Const MTS_SLV_V As Short = &H4000S ' Slave axis move.
    Public Const MTS_JOG_V As Short = &H8000S ' Jog move.
    Public Const MTS_ASTP_V As Integer = &H10000 ' Abnormal stop.
    Public Const MTS_SVONS_V As Integer = &H20000 ' Servo off stopped.
    Public Const MTS_EMGS_V As Integer = &H40000 ' EMG / SEMG stopped.
    Public Const MTS_ALMS_V As Integer = &H80000 ' Alarm stop.
    Public Const MTS_WANS_V As Integer = &H100000 ' Warning stopped.
    Public Const MTS_PELS_V As Integer = &H200000 ' PEL stopped.
    Public Const MTS_MELS_V As Integer = &H400000 ' MEL stopped.
    Public Const MTS_ECES_V As Integer = &H800000 ' Error counter check level reaches and stopped.
    Public Const MTS_SPELS_V As Integer = &H1000000 ' Soft PEL stopped.
    Public Const MTS_SMELS_V As Integer = &H2000000 ' Soft MEL stopped.
    Public Const MTS_STPOA_V As Integer = &H4000000 ' Stop by others axes.
    Public Const MTS_GDCES_V As Integer = &H8000000 ' Gantry deviation error level reaches and stopped.
    Public Const MTS_GTM_V As Integer = &H10000000 ' Gantry mode turn on.
    Public Const MTS_PAPB_V As Integer = &H20000000 ' Pulsar mode turn on.
    
    'PointTable, option
		Public Const PT_OPT_ABS      As Integer = &H00000000    '// move, absolute
		Public Const PT_OPT_REL      As Integer = &H00000001    '// move, relative
		Public Const PT_OPT_LINEAR   As Integer = &H00000000    '// move, linear
		Public Const PT_OPT_ARC      As Integer = &H00000004   '// move, arc
		Public Const PT_OPT_FC_CSTP  As Integer = &H00000000    '// signal, command stop (finish condition)
		Public Const PT_OPT_FC_INP   As Integer = &H00000010    '// signal, in position
		Public Const PT_OPT_LAST_POS As Integer = &H00000020    '// last point index
		Public Const PT_OPT_DWELL    As Integer = &H00000040   '// dwell
		Public Const PT_OPT_RAPID    As Integer = &H00000080    '// rapid positioning
		Public Const PT_OPT_NOARC    As Integer = &H00010000    '// do not add arc
		Public Const PT_OPT_SCUVE    As Integer = &H00000002    '// s-curve

    'Trigger parameter number define. [Only for PCI-8258]   
    Public Const TGR_LCMP0_SRC As Short = &H0S
    Public Const TGR_LCMP1_SRC As Short = &H1S
    Public Const TGR_TCMP0_SRC As Short = &H2S
    Public Const TGR_TCMP1_SRC As Short = &H3S

    Public Const TGR_TCMP0_DIR As Short = &H4S
    Public Const TGR_TCMP1_DIR As Short = &H5S
    Public Const TGR_TRG_EN As Short = &H6S

    Public Const TGR_TRG0_SRC As Short = &H10S
    Public Const TGR_TRG1_SRC As Short = &H11S
    Public Const TGR_TRG2_SRC As Short = &H12S
    Public Const TGR_TRG3_SRC As Short = &H13S

    Public Const TGR_TRG0_PWD As Short = &H14S
    Public Const TGR_TRG1_PWD As Short = &H15S
    Public Const TGR_TRG2_PWD As Short = &H16S
    Public Const TGR_TRG3_PWD As Short = &H17S

    Public Const TGR_TRG0_LOGIC As Short = &H18S
    Public Const TGR_TRG1_LOGIC As Short = &H19S
    Public Const TGR_TRG2_LOGIC As Short = &H1AS
    Public Const TGR_TRG3_LOGIC As Short = &H1BS

    Public Const TGR_TRG0_TGL As Short = &H1CS
    Public Const TGR_TRG1_TGL As Short = &H1DS
    Public Const TGR_TRG2_TGL As Short = &H1ES
    Public Const TGR_TRG3_TGL As Short = &H1FS

    Public Const TIMR_ITV As Short = &H20S
    Public Const TIMR_DIR As Short = &H21S
    Public Const TIMR_RING_EN As Short = &H22S
    Public Const TIMR_EN As Short = &H23S
    
    '//Trigger parameter number define. [Only for PCI-8158A]
		Public Const TIG_LCMP0_SRC As Short = &H00S
		Public Const TIG_LCMP1_SRC As Short = &H01S
		Public Const TIG_LCMP2_SRC As Short = &H02S
		Public Const TIG_LCMP3_SRC As Short = &H03S
		Public Const TIG_LCMP4_SRC As Short = &H04S
		Public Const TIG_LCMP5_SRC As Short = &H05S
		Public Const TIG_LCMP6_SRC As Short = &H06S
		Public Const TIG_LCMP7_SRC As Short = &H07S
		Public Const TIG_TCMP0_SRC As Short = &H08S
		Public Const TIG_TCMP1_SRC As Short = &H09S
		Public Const TIG_TCMP2_SRC As Short = &H0AS
		Public Const TIG_TCMP3_SRC As Short = &H0BS
		Public Const TIG_TCMP4_SRC As Short = &H0CS
		Public Const TIG_TCMP5_SRC As Short = &H0DS
		Public Const TIG_TCMP6_SRC As Short = &H0ES
		Public Const TIG_TCMP7_SRC As Short = &H0FS
		Public Const TIG_TRG0_EN As Short = &H10S
		Public Const TIG_TRG1_EN As Short = &H11S
		Public Const TIG_TRG2_EN As Short = &H12S
		Public Const TIG_TRG3_EN As Short = &H13S
		Public Const TIG_TRG4_EN As Short = &H14S
		Public Const TIG_TRG5_EN As Short = &H15S
		Public Const TIG_TRG6_EN As Short = &H16S
		Public Const TIG_TRG7_EN As Short = &H17S
		Public Const TIG_TRG0_SRC As Short = &H18S
		Public Const TIG_TRG1_SRC As Short = &H19S
		Public Const TIG_TRG2_SRC As Short = &H1AS
		Public Const TIG_TRG3_SRC As Short = &H1BS
		Public Const TIG_TRG4_SRC As Short = &H1CS
		Public Const TIG_TRG5_SRC As Short = &H1DS
		Public Const TIG_TRG6_SRC As Short = &H1ES
		Public Const TIG_TRG7_SRC As Short = &H1FS
		Public Const TIG_TRG0_PWD As Short = &H20S
		Public Const TIG_TRG1_PWD As Short = &H21S
		Public Const TIG_TRG2_PWD As Short = &H20S
		Public Const TIG_TRG3_PWD As Short = &H23S
		Public Const TIG_TRG4_PWD As Short = &H24S
		Public Const TIG_TRG5_PWD As Short = &H25S
		Public Const TIG_TRG6_PWD As Short = &H26S
		Public Const TIG_TRG7_PWD As Short = &H27S
		Public Const TIG_TRG0_LOGIC As Short = &H28S
		Public Const TIG_TRG1_LOGIC As Short = &H29S
		Public Const TIG_TRG2_LOGIC As Short = &H2AS
		Public Const TIG_TRG3_LOGIC As Short = &H2BS
		Public Const TIG_TRG4_LOGIC As Short = &H2CS
		Public Const TIG_TRG5_LOGIC As Short = &H2DS
		Public Const TIG_TRG6_LOGIC As Short = &H2ES
		Public Const TIG_TRG7_LOGIC As Short = &H2FS
		Public Const TIG_TRG0_TGL As Short = &H30S
		Public Const TIG_TRG1_TGL As Short = &H31S
		Public Const TIG_TRG2_TGL As Short = &H32S
		Public Const TIG_TRG3_TGL As Short = &H33S
		Public Const TIG_TRG4_TGL As Short = &H34S
		Public Const TIG_TRG5_TGL As Short = &H35S
		Public Const TIG_TRG6_TGL As Short = &H36S
		Public Const TIG_TRG7_TGL As Short = &H37S
		Public Const TIG_PWMTMR0_ITV As Short = &H40S
		Public Const TIG_PWMTMR1_ITV As Short = &H41S
		Public Const TIG_PWMTMR2_ITV As Short = &H42S
		Public Const TIG_PWMTMR3_ITV As Short = &H43S
		Public Const TIG_PWMTMR4_ITV As Short = &H44S
		Public Const TIG_PWMTMR5_ITV As Short = &H45S
		Public Const TIG_PWMTMR6_ITV As Short = &H46S
		Public Const TIG_PWMTMR7_ITV As Short = &H47S
		Public Const TIG_TMR0_ITV As Short = &H50S
		Public Const TIG_TMR0_DIR As Short = &H51S

    '// move option define
    Public Const OPT_ABSOLUTE As Integer = &H0
    Public Const OPT_RELATIVE As Integer = &H1
    Public Const OPT_WAIT As Integer = &H100

    '// PTP buffer mode define
    Public Const PTP_OPT_ABORTING As Integer = &H0S
    Public Const PTP_OPT_BUFFERED As Integer = &H1000S
    Public Const PTP_OPT_BLEND_LOW As Integer = &H2000S
    Public Const PTP_OPT_BLEND_PREVIOUS As Integer = &H3000
    Public Const PTP_OPT_BLEND_NEXT As Integer = &H4000S
    Public Const PTP_OPT_BLEND_HIGH As Integer = &H5000S

    Public Const ITP_OPT_ABORT_BLEND As Integer = &H0S
    Public Const ITP_OPT_ABORT_FORCE As Integer = &H1000S
    Public Const ITP_OPT_ABORT_STOP As Integer = &H2000S
    Public Const ITP_OPT_BUFFERED As Integer = &H3000S
    Public Const ITP_OPT_BLEND_DEC_EVENT As Integer = &H4000S
    Public Const ITP_OPT_BLEND_RES_DIST As Integer = &H5000S
    Public Const ITP_OPT_BLEND_RES_DIST_PERCENT As Integer = &H6000S
    
    'Latch parameter number define. [Only for PCI-8158A]
		Public Const LTC_ENC_IPT_MODE          As Integer = &H00
		Public Const LTC_ENC_EA_INV            As Integer = &H01
		Public Const LTC_ENC_EB_INV            As Integer = &H02
		Public Const LTC_ENC_EZ_CLR_LOGIC      As Integer = &H03
		Public Const LTC_ENC_EZ_CLR_EN         As Integer = &H04
		Public Const LTC_ENC_SIGNAL_FILITER_EN As Integer = &H05
		Public Const LTC_FIFO_HIGH_LEVEL       As Integer = &H06
		Public Const LTC_SIGNAL_FILITER_EN     As Integer = &H07
		Public Const LTC_SIGNAL_TRIG_LOGIC     As Integer = &H08

    'Error code define
    Public Const ERR_NoError						=(0)		'//No Error	
		Public Const ERR_OSVersion					=(-1)	'// Operation System type mismatched
		Public Const ERR_OpenDriverFailed			=(-2)'	// Open device driver failed - Create driver interface failed
		Public Const ERR_InsufficientMemory		=(-3)	'// System memory insufficiently
		Public Const ERR_DeviceNotInitial			=(-4)	'// Cards not be initialized
		Public Const ERR_NoDeviceFound			=	(-5)	'// Cards not found(No card in your system)
		Public Const ERR_CardIdDuplicate				=(-6)'	// Cards' ID is duplicated. 
		Public Const ERR_DeviceAlreadyInitialed	=	(-7)'	// Cards have been initialed 
		Public Const ERR_InterruptNotEnable			=(-8)'	// Cards' interrupt events not enable or not be initialized
		Public Const ERR_TimeOut					=	(-9)	'// Function time out
		Public Const ERR_ParametersInvalid		=	(-10)'	// Function input parameters are invalid
		Public Const ERR_SetEEPROM					=(-11)	'// Set data to EEPROM (or nonvolatile memory) failed
		Public Const ERR_GetEEPROM					=(-12)	'// Get data from EEPROM (or nonvolatile memory) failed
		Public Const ERR_FunctionNotAvailable		=(-13)'	// Function is not available in this step, The device is not support this function or Internal process failed
		Public Const ERR_FirmwareError			=	(-14)   '// Firmware error, please reboot the system
		Public Const ERR_CommandInProcess		=	(-15)	'// Previous command is in process
		Public Const ERR_AxisIdDuplicate			=	(-16)'	// Axes' ID is duplicated.
		Public Const ERR_ModuleNotFound				=(-17)  ' // Slave module not found.
		Public Const ERR_InsufficientModuleNo		=(-18)	'// System ModuleNo insufficiently
		Public Const ERR_HandShakeFailed			=	(-19)   '// HandSake with the DSP out of time.
		Public Const ERR_FILE_FORMAT					=(-20)	'// Config file format error.(cannot be parsed)
		Public Const ERR_ParametersReadOnly		=	(-21)	'// Function parameters read only.
		Public Const ERR_DistantNotEnough			=(-22)	'// Distant is not enough for motion.
		Public Const ERR_FunctionNotEnable		=	(-23)	'// Function is not enabled.
		Public Const ERR_ServerAlreadyClose		=(-24)	'// Server already closed.
		Public Const ERR_DllNotFound					=(-25)	'// Related dll is not found, not in correct path.
		Public Const ERR_TrimDAC_Channel		=		(-26)
		Public Const ERR_Satellite_Type				=(-27)
		Public Const ERR_Over_Voltage_Spec		=	(-28)
		Public Const ERR_Over_Current_Spec		=	(-29)
		Public Const ERR_SlaveIsNotAI			=	(-30)
		Public Const ERR_Over_AO_Channel_Scope	=	(-31)
		Public Const ERR_DllFuncFailed				=(-32)	'// Failed to invoke dll function. Extension Dll version is wrong.
		Public Const ERR_FeederAbnormalStop		=(-33) '//Feeder abnormal stop, External stop or feeding stop
		Public Const ERR_Read_ModuleType_Dismatch=	(-34)
		Public Const ERR_Win32Error				=	(-1000) '// No such INT number, or WIN32_API error, contact with ADLINK's FAE staff.
		Public Const ERR_DspStart				=	(-2000) '// The base for DSP error



End Module