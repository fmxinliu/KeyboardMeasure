Option Strict Off
Option Explicit On
Module APS168
    '  Copyright  Lib "APS168x64.dll" (C) 1995-2009 Adlink Technology INC.
    '  All rights reserved.
    'APS type_def+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    Structure STR_SAMP_DATA_4CH
        Dim tick As Short
        Dim data_0 As Short 'Total channel = 4
        Dim data_1 As Short 'Total channel = 4
        Dim data_2 As Short 'Total channel = 4
        Dim data_3 As Short 'Total channel = 4
    End Structure

    Structure STR_SAMP_DATA_8CH
        Dim tick As Int32
        Dim data_0 As Int32 'Total channel = 4
        Dim data_1 As Int32 'Total channel = 4
        Dim data_2 As Int32 'Total channel = 4
        Dim data_3 As Int32 'Total channel = 4
        Dim data_4 As Int32 'Total channel = 4
        Dim data_5 As Int32 'Total channel = 4
        Dim data_6 As Int32 'Total channel = 4
        Dim data_7 As Int32 'Total channel = 4
    End Structure

    Structure MOVE_PARA 'Speed pattern
        Dim i16_accType As Short 'Axis parameter
        Dim i16_decType As Short 'Axis parameter
        Dim i32_acc As Integer 'Axis parameter
        Dim i32_dec As Integer 'Axis parameter
        Dim i32_initSpeed As Integer 'Axis parameter
        Dim i32_maxSpeed As Integer 'Axis parameter
        Dim i32_endSpeed As Integer 'Axis parameter
    End Structure

    Structure POINT_DATA
        Dim i32_pos As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse)
        Dim i16_accType As Short ' Acceleration pattern 0: T-curve,  1: S-curve
        Dim i16_decType As Short ' Deceleration pattern 0: T-curve,  1: S-curve
        Dim i32_acc As Integer ' Acceleration rate  Lib "APS168x64.dll" ( pulse / ss )
        Dim i32_dec As Integer ' Deceleration rate  Lib "APS168x64.dll" ( pulse / ss )
        Dim i32_initSpeed As Integer ' Start velocity         Lib "APS168x64.dll" ( pulse / s )
        Dim i32_maxSpeed As Integer ' Maximum velocity   Lib "APS168x64.dll" ( pulse / s )
        Dim i32_endSpeed As Integer ' End velocity           Lib "APS168x64.dll" ( pulse / s )
        Dim i32_angle As Integer ' Arc move angle     Lib "APS168x64.dll" ( degree, -360 ~ 360 )
        Dim u32_dwell As Integer ' Dwell times        Lib "APS168x64.dll" ( unit: ms )
        Dim i32_opt As Integer ' Option '0xABCD , D:0 absolute, 1:relative
    End Structure

    Structure POINT_DATA_EX
        Dim i32_pos As Integer         '//(Center)Position data (could be relative or absolute value) 
        Dim i16_accType As Short       '//Acceleration pattern 0: T curve, 1:S curve   
        Dim i16_decType As Short       '// Deceleration pattern 0: T curve, 1:S curve 
        Dim i32_acc As Integer           '  //Acceleration rate ( pulse / sec 2 ) 
        Dim i32_dec As Integer          '   //Deceleration rate ( pulse / sec 2  ) 
        Dim i32_initSpeed As Integer   '  //Start velocity ( pulse / s ) 
        Dim i32_maxSpeed As Integer  '//Maximum velocity    ( pulse / s ) 
        Dim i32_endSpeed As Integer   '//End velocity  ( pulse / s )     
        Dim i32_angle As Integer         ' //Arc move angle ( degree, -360 ~ 360 ) 
        Dim u32_dwell As Integer         '//dwell times ( unit: ms ) *Divided by system cycle time. 
        Dim i32_opt As Integer           '//Point move option. (*) 
        Dim i32_pitch As Integer               '// pitch for helical move
        Dim i32_totalheight As Integer   '// total hight
        Dim i16_cw As Short                    '// cw or ccw
        Dim i16_opt_ext As Short             '// option extend
    End Structure

    Structure POINT_DATA2
        Dim i32_pos_0 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_1 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_2 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_3 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_4 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_5 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_6 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_7 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_8 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_9 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_10 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_11 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_12 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_13 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_14 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_pos_15 As Integer ' Position data  Lib "APS168x64.dll" (relative or absolute)  Lib "APS168x64.dll" (pulse) , Arraysize = 16
        Dim i32_initSpeed As Integer ' Start velocity         Lib "APS168x64.dll" ( pulse / s )
        Dim i32_maxSpeed As Integer ' Maximum velocity   Lib "APS168x64.dll" ( pulse / s )
        Dim i32_angle As Integer ' Arc move angle     Lib "APS168x64.dll" ( degree, -360 ~ 360 )
        Dim u32_dwell As Integer ' Dwell times        Lib "APS168x64.dll" ( unit: ms )
        Dim i32_opt As Integer ' Option '0xABCD , D:0 absolute, 1:relative
    End Structure


    Structure POINT_DATA3
        Dim i32_pos_0 As Integer
        Dim i32_pos_1 As Integer
        Dim i32_pos_2 As Integer
        Dim i32_pos_3 As Integer
        Dim i32_maxSpeed As Integer

        Dim i32_endPos_0 As Integer
        Dim i32_endPos_1 As Integer

        Dim i32_dir As Integer
        Dim i32_opt As Integer
    End Structure

    Structure JOG_DATA
        Dim i16_jogMode As Short ' Jog mode. 0:Free running mode, 1:Step mode
        Dim i16_dir As Short ' Jog direction. 0:positive, 1:negative direction
        Dim i16_accType As Short ' Acceleration pattern 0: T-curve,  1: S-curve
        Dim i32_acc As Integer ' Acceleration rate  Lib "APS168x64.dll" ( pulse / ss )
        Dim i32_dec As Integer ' Deceleration rate  Lib "APS168x64.dll" ( pulse / ss )
        Dim i32_maxSpeed As Integer ' Positive value, maximum velocity   Lib "APS168x64.dll" ( pulse / s )
        Dim i32_offset As Integer ' Positive value, a step  Lib "APS168x64.dll" (pulse)
        Dim i32_delayTime As Integer ' Delay time,  Lib "APS168x64.dll" ( range: 0 ~ 65535 millisecond, align by cycle time)
    End Structure

    Structure HOME_PARA
        Dim u8_homeMode As Byte
        Dim u8_homeDir As Byte
        Dim u8_curveType As Byte
        Dim i32_orgOffset As Integer
        Dim i32_acceleration As Integer
        Dim i32_startVelocity As Integer
        Dim i32_maxVelocity As Integer
        Dim i32_OrgVelocity As Integer
    End Structure

    Structure FILTER_COEF
        Dim a1 As Double ' Biquad filter output polynomial coefficient
        Dim a2 As Double ' Biquad filter output polynomial coefficient
        Dim b0 As Double ' Biquad filter input polynomial coefficient
        Dim b1 As Double ' Biquad filter input polynomial coefficient
        Dim b2 As Double ' Biquad filter input polynomial coefficient
    End Structure
    Structure POS_DATA_2D
        Dim u32_opt As Integer         'option, [0x00000000,0xFFFFFFFF]
        Dim i32_x As Integer           ' x-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
        Dim i32_y As Integer           'y-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
        Dim i32_theta As Integer       'x-y plane arc move angle  Lib "APS168x64.dll" (0.001 degree), [-360000,360000]
    End Structure

    Structure PNT_DATA_2D
        Dim u32_opt As Integer        ' option, [0x00000000,0xFFFFFFFF]
        Dim i32_x As Integer          ' x-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
        Dim i32_y As Integer          ' y-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
        Dim i32_theta As Integer      ' x-y plane arc move angle  Lib "APS168x64.dll" (0.001 degree), [-360000,360000]
        Dim i32_acc As Integer        ' acceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
        Dim i32_dec As Integer        ' deceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
        Dim i32_vi As Integer         ' initial velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
        Dim i32_vm As Integer         ' maximum velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
        Dim i32_ve As Integer         ' ending velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
    End Structure

    Structure PNT_DATA_2D_F64
        Dim u32_opt As Integer        '// option, [0x00000000,0xFFFFFFFF]
        Dim f64_x As Double          '// x-axis component (pulse), [-2147483648,2147484647]
        Dim f64_y As Double          '// y-axis component (pulse), [-2147483648,2147484647]
        Dim f64_theta As Double      '// x-y plane arc move angle (0.000001 degree), [-360000,360000]
        Dim f64_acc As Double        '// acceleration rate (pulse/ss), [0,2147484647]
        Dim f64_dec As Double        '// deceleration rate (pulse/ss), [0,2147484647]
        Dim f64_vi As Double         '// initial velocity (pulse/s), [0,2147484647]
        Dim f64_vm As Double         '// maximum velocity (pulse/s), [0,2147484647]
        Dim f64_ve As Double         '// ending velocity (pulse/s), [0,2147484647]
        Dim f64_sf As Double             ' // s-factor [0.0 ~ 1.0]
    End Structure










    ' Point table structure  Lib "APS168x64.dll" (Four dimension)
    Structure PNT_DATA_4DL
        Dim u32_opt As Integer        ' option, [0x00000000,0xFFFFFFFF]
        Dim i32_x As Integer          ' x-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
        Dim i32_y As Integer          ' y-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
        Dim i32_z As Integer          ' z-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
        Dim i32_u As Integer          ' u-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
        Dim i32_acc As Integer        ' acceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
        Dim i32_dec As Integer        ' deceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
        Dim i32_vi As Integer         ' initial velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
        Dim i32_vm As Integer         ' maximum velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
        Dim i32_ve As Integer         ' ending velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
    End Structure


    ' Point table structure  Lib "APS168x64.dll" (One dimension)
    Structure PNT_DATA
        Dim u32_opt As Integer        ' option, [0x00000000,0xFFFFFFFF]
        Dim i32_x As Integer          ' x-axis component  Lib "APS168x64.dll" (pulse), [-2147483648,2147484647]
        Dim i32_theta As Integer      ' x-y plane arc move angle  Lib "APS168x64.dll" (0.001 degree), [-360000,360000]
        Dim i32_acc As Integer        ' acceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
        Dim i32_dec As Integer        ' deceleration rate  Lib "APS168x64.dll" (pulse/ss), [0,2147484647]
        Dim i32_vi As Integer         ' initial velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
        Dim i32_vm As Integer         ' maximum velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
        Dim i32_ve As Integer         ' ending velocity  Lib "APS168x64.dll" (pulse/s), [0,2147484647]
    End Structure
    '//Asynchronized call
    Structure ASYNCALL
        'HANDLE	h_event; // IntPtr
        Dim h_event As IntPtr
        Dim i32_ret As Integer
    End Structure
    Structure TSK_INFO
        Dim State As UShort
        Dim RunTimeErr As UShort
        Dim IP As UShort
        Dim SP As UShort
        Dim BP As UShort
        Dim MsgQueueSts As UShort
    End Structure
    Structure VAO_DATA
        'Param
        Dim outputType As Integer 'Output type, [0, 3]
        Dim inputType As Integer 'Input type, [0, 1]
        Dim config As Integer 'PWM configuration according to output type
        Dim inputSrc As Integer 'Input source by axis, [0, 0xf]
        'Mapping table
        Dim minVel As Integer 'Minimum linear speed, [ positive ]
        Dim velInterval As Integer 'Speed interval, [ positive ]
        Dim totalPoints As Integer 'Total points, [1, 32]
        Dim mappingDataArr_0 As Integer 'mapping data array
        Dim mappingDataArr_1 As Integer 'mapping data array
        Dim mappingDataArr_2 As Integer 'mapping data array
        Dim mappingDataArr_3 As Integer 'mapping data array
        Dim mappingDataArr_4 As Integer 'mapping data array
        Dim mappingDataArr_5 As Integer 'mapping data array
        Dim mappingDataArr_6 As Integer 'mapping data array
        Dim mappingDataArr_7 As Integer 'mapping data array
        Dim mappingDataArr_8 As Integer 'mapping data array
        Dim mappingDataArr_9 As Integer 'mapping data array
        Dim mappingDataArr_10 As Integer 'mapping data array
        Dim mappingDataArr_11 As Integer 'mapping data array
        Dim mappingDataArr_12 As Integer 'mapping data array
        Dim mappingDataArr_13 As Integer 'mapping data array
        Dim mappingDataArr_14 As Integer 'mapping data array
        Dim mappingDataArr_15 As Integer 'mapping data array
        Dim mappingDataArr_16 As Integer 'mapping data array
        Dim mappingDataArr_17 As Integer 'mapping data array
        Dim mappingDataArr_18 As Integer 'mapping data array
        Dim mappingDataArr_19 As Integer 'mapping data array
        Dim mappingDataArr_20 As Integer 'mapping data array
        Dim mappingDataArr_21 As Integer 'mapping data array
        Dim mappingDataArr_22 As Integer 'mapping data array
        Dim mappingDataArr_23 As Integer 'mapping data array
        Dim mappingDataArr_24 As Integer 'mapping data array
        Dim mappingDataArr_25 As Integer 'mapping data array
        Dim mappingDataArr_26 As Integer 'mapping data array
        Dim mappingDataArr_27 As Integer 'mapping data array
        Dim mappingDataArr_28 As Integer 'mapping data array
        Dim mappingDataArr_29 As Integer 'mapping data array
        Dim mappingDataArr_30 As Integer 'mapping data array
        Dim mappingDataArr_31 As Integer 'mapping data array

    End Structure

    Public Const MAX_SAMPL_CH = (8)
    Public Const MAX_SAMPL_SRC = (2)

    <System.Serializable()> <System.Runtime.InteropServices.ComVisible(False)> Structure SAMP_PARAM

        Dim rate As Integer             '//Sampling rate
        Dim edge As Integer             '//Trigger edge
        Dim level As Integer            '//Trigger level
        Dim trigCh As Integer           '//Trigger channel
        Dim sourceByCh_0_0 As Integer
        Dim sourceByCh_0_1 As Integer
        Dim sourceByCh_1_0 As Integer
        Dim sourceByCh_1_1 As Integer
        Dim sourceByCh_2_0 As Integer
        Dim sourceByCh_2_1 As Integer
        Dim sourceByCh_3_0 As Integer
        Dim sourceByCh_3_1 As Integer
        Dim sourceByCh_4_0 As Integer
        Dim sourceByCh_4_1 As Integer
        Dim sourceByCh_5_0 As Integer
        Dim sourceByCh_5_1 As Integer
        Dim sourceByCh_6_0 As Integer
        Dim sourceByCh_6_1 As Integer
        Dim sourceByCh_7_0 As Integer
        Dim sourceByCh_7_1 As Integer
        '//Sampling source by channel, named sourceByCh[a][b], 
        '//a: Channel
        '//b: 0: Sampling source 1: Sampling axis
        '//Sampling source: F64 data occupies two channel, I32 data occupies one channel.
    End Structure

    Public Const MAX_PT_DIM = (6)

    Structure PTINFO
        Dim Dimension As Integer
        'Dim AxisArr%(MAX_PT_DIM) As Integer
        Dim AxisArr_0 As Integer
        Dim AxisArr_1 As Integer
        Dim AxisArr_2 As Integer
        Dim AxisArr_3 As Integer
        Dim AxisArr_4 As Integer
        Dim AxisArr_5 As Integer
    End Structure

    Structure PTDWL
        Dim DwTime As Double '//Unit is ms
    End Structure

    Structure PTLINE
        Dim Dimension As Integer
        'Dim Pos#(MAX_PT_DIM) As Double
        Dim Pos_0 As Double
        Dim Pos_1 As Double
        Dim Pos_2 As Double
        Dim Pos_3 As Double
        Dim Pos_4 As Double
        Dim Pos_5 As Double
    End Structure

    Public Const MAXHEXLIX = &H3 'Helix axes is 3.
    Public Const MAXARC3 = &H3 'ARC3 axes is 3.
    Public Const MAXARC2 = &H2 'ARC2 axes is 2.

    Structure PTA2CA
        'Dim Index(MAXARC2) As Byte 'Index X,Y
        Dim Index_0 As Byte
        Dim Index_1 As Byte
        'Dim Center#(MAXARC2) As Double 'Center Arr
        Dim Center_0 As Double
        Dim Center_1 As Double
        Dim Angle As Double 'Angle
    End Structure

    Structure PTA2CE
        'Dim Index(MAXARC2) As Byte 'Index X,Y
        Dim index_0 As Byte
        Dim index_1 As Byte
        'Dim Center#(MAXARC2) As Double
        Dim Center_0 As Double
        Dim Center_1 As Double
        'Dim End_pos(MAXARC2) As Double
        Dim End_pos_0 As Double
        Dim End_pos_1 As Double
        Dim Dir As Short
    End Structure
    Structure PTA3CA
        'Dim Index(MAXARC3) As Byte 'Index X,Y
        Dim Index_0 As Byte
        Dim Index_1 As Byte
        Dim Index_2 As Byte
        'Dim Center#(MAXARC3) As Double 'Center Arr
        Dim Center_0 As Double
        Dim Center_1 As Double
        Dim Center_2 As Double
        'Dim Noraml(MAXARC3) As Double 'Normal Arr
        Dim Noraml_0 As Double
        Dim Noraml_1 As Double
        Dim Noraml_2 As Double
        Dim Angle As Double 'Angle
    End Structure

    Structure PTA3CE
        'Dim Index(MAXARC3) As Byte 'Index X,Y
        Dim index_0 As Byte
        Dim index_1 As Byte
        Dim index_2 As Byte
        'Dim Center#(MAXARC3) As Double
        Dim Center_0 As Double
        Dim Center_1 As Double
        Dim Center_2 As Double
        'Dim End_pos(MAXARC3) As Double
        Dim End_pos_0 As Double
        Dim End_pos_1 As Double
        Dim End_pos_2 As Double
        Dim Dir As Short
    End Structure

    Structure PTHCA
        'Dim Index(MAXHEXLIX) As Byte 'Index X,Y
        Dim Index_0 As Byte
        Dim Index_1 As Byte
        Dim Index_2 As Byte
        'Dim Center(MAXHEXLIX) As Double 'Center Arr
        Dim Center_0 As Double
        Dim Center_1 As Double
        Dim Center_2 As Double
        'Dim Noraml(MAXHEXLIX) As Double 'Normal Arr
        Dim Noraml_0 As Double
        Dim Noraml_1 As Double
        Dim Noraml_2 As Double
        Dim Angle As Double 'Angle
        Dim DeltaH As Double
        Dim FinalR As Double
    End Structure

    Structure PTHCE
        'Dim Index(MAXHEXLIX) As Byte 'Index X,Y
        Dim Index_0 As Byte
        Dim Index_1 As Byte
        Dim Index_2 As Byte
        'Dim Center(MAXHEXLIX) As Double 'Center Arr
        Dim Center_0 As Double
        Dim Center_1 As Double
        Dim Center_2 As Double
        'Dim Noraml(MAXHEXLIX) As Double 'Normal Arr
        Dim Noraml_0 As Double
        Dim Noraml_1 As Double
        Dim Noraml_2 As Double
        'Dim End_pos(MAXHEXLIX) As Double 'End Arr
        Dim End_pos_0 As Double
        Dim End_pos_1 As Double
        Dim End_pos_2 As Double
        Dim Dir As Short
    End Structure
    Structure PTSTS
        Dim BitSts As UShort 'b0: Is PTB work? [1:working, 0:Stopped]
        'b1: Is point buffer full? [1:full, 0:not full]
        'b2: Is point buffer empty? [1:empty, 0:not empty]
        'b3, b4, b5: Reserved for future
        'b6~: Be always 0
        Dim PntBufFreeSpace As UShort
        Dim PntBufUsageSpace As UShort
        Dim RunningCnt As UInteger
    End Structure

    Structure LPSTS
        Dim MotionLoopLoading As UInteger
        Dim HostLoopLoading As UInteger
        Dim MotionLoopLoadingMax As UInteger
        Dim HostLoopLoadingMax As UInteger
    End Structure

    Structure DEBUG_DATA
        Dim ServoOffCondition As UInteger
        Dim DspCmdPos As Double
        Dim DspFeedbackPos As Double
        Dim FpgaCmdPos As Double
        Dim FpgaFeedbackPos As Double
        Dim FpgaOutputVoltage As Double
    End Structure

    Structure DEBUG_STATE
        Dim AxisState As UInteger
        Dim GroupState As UInteger
        Dim AxisSuperState As UInteger
    End Structure

    'New ADCNC structure define
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Structure POS_DATA_2D_F64
        ' This structure extends original point data contents from "I32" to "F64" 
        'for internal computation. It's important to prevent data overflow.

        Dim u32_opt As UInteger        '// option, [0x00000000, 0xFFFFFFFF]
        Dim f64_x As Double          '// x-axis component (pulse), [-9223372036854775808, 9223372036854775807]
        Dim f64_y As Double          '// y-axis component (pulse), [-9223372036854775808, 9223372036854775807]
        Dim f64_theta As Double      '// x-y plane arc move angle (0.000001 degree), [-360000, 360000]
    End Structure

    Structure POS_DATA_2D_RPS
        '/* This structure adds another variable to record what point was be saved */		
        Dim u32_opt As UInteger        '// option, [0x00000000, 0xFFFFFFFF]
        Dim i32_x As Integer          '// x-axis component (pulse), [-2147483648, 2147483647]
        Dim i32_y As Integer          '// y-axis component (pulse), [-2147483648, 2147483647]
        Dim i32_theta As Integer      '// x-y plane arc move angle (0.000001 degree), [-360000, 360000]
        Dim crpi As UInteger                  '// current reading point index
    End Structure

    Structure POS_DATA_2D_F64_RPS
        '/* This structure adds another variable to record what point was be saved */
        Dim u32_opt As UInteger        '// option, [0x00000000, 0xFFFFFFFF]
        Dim f64_x As Double          '// x-axis component (pulse), [-2147483648, 2147483647]
        Dim f64_y As Double          '// y-axis component (pulse), [-2147483648, 2147483647]
        Dim f64_theta As Double      '// x-y plane arc move angle (0.000001 degree), [-360000, 360000]
        Dim crpi As UInteger           '// current reading point index
    End Structure

    Structure PNT_DATA_2D_EXT
        Dim u32_opt As UInteger        '// option, [0x00000000,0xFFFFFFFF]
        Dim f64_x As Double          '// x-axis component (pulse), [-2147483648,2147484647]
        Dim f64_y As Double         ' // y-axis component (pulse), [-2147483648,2147484647]
        Dim f64_theta As Double     ' // x-y plane arc move angle (0.000001 degree), [-360000,360000]  
        Dim f64_acc_0 As Double     '   // acceleration rate (pulse/ss), [0,2147484647]
        Dim f64_acc_1 As Double     '   // acceleration rate (pulse/ss), [0,2147484647]
        Dim f64_acc_2 As Double     '   // acceleration rate (pulse/ss), [0,2147484647]
        Dim f64_acc_3 As Double     '   // acceleration rate (pulse/ss), [0,2147484647]

        Dim f64_dec_0 As Double      '  // deceleration rate (pulse/ss), [0,2147484647]
        Dim f64_dec_1 As Double      '  // deceleration rate (pulse/ss), [0,2147484647]
        Dim f64_dec_2 As Double      '  // deceleration rate (pulse/ss), [0,2147484647]
        Dim f64_dec_3 As Double      '  // deceleration rate (pulse/ss), [0,2147484647]
        Dim crossover As Integer
        Dim Iboundary As Integer   '	// initial boundary
        Dim f64_vi_0 As Double   '   // initial velocity (pulse/s), [0,2147484647]
        Dim f64_vi_1 As Double   '   // initial velocity (pulse/s), [0,2147484647]
        Dim f64_vi_2 As Double   '   // initial velocity (pulse/s), [0,2147484647]
        Dim f64_vi_3 As Double   '   // initial velocity (pulse/s), [0,2147484647]
        Dim vi_cmpr As UInteger
        Dim f64_vm_0 As Double     ' // maximum velocity (pulse/s), [0,2147484647]
        Dim f64_vm_1 As Double     ' // maximum velocity (pulse/s), [0,2147484647]
        Dim f64_vm_2 As Double     ' // maximum velocity (pulse/s), [0,2147484647]
        Dim f64_vm_3 As Double     ' // maximum velocity (pulse/s), [0,2147484647]
        Dim vm_cmpr As UInteger
        Dim f64_ve_0 As Double      '// ending velocity (pulse/s), [0,2147484647]
        Dim f64_ve_1 As Double      '// ending velocity (pulse/s), [0,2147484647]
        Dim f64_ve_2 As Double      '// ending velocity (pulse/s), [0,2147484647]
        Dim f64_ve_3 As Double      '// ending velocity (pulse/s), [0,2147484647]
        Dim ve_cmpr As UInteger
        Dim Eboundary As Integer       '// end boundary
        Dim f64_dist As Double        '// point distance
        Dim f64_angle As Double        '// path angle between previous & current point
        Dim f64_radius As Double      '// point radiua (used in arc move)
        Dim i32_arcstate As Integer
        Dim spt As UInteger           '// speed profile type

        '// unit time measured by DSP sampling period
        Dim t_0 As Double
        Dim t_1 As Double
        Dim t_2 As Double
        Dim t_3 As Double

        '// Horizontal & Vertical line flag
        Dim HorizontalFlag As Integer
        Dim VerticalFlag As Integer
    End Structure
    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    'APS Library+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    '  System & Initialization
    Declare Function APS_initial Lib "APS168x64.dll" (ByRef BoardID_InBits As Integer, ByVal Mode As Integer) As Integer
    Declare Function APS_close Lib "APS168x64.dll" () As Integer
    Declare Function APS_version Lib "APS168x64.dll" () As Integer
    Declare Function APS_device_driver_version Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_get_axis_info Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Board_ID As Integer, ByRef Axis_No As Integer, ByRef Port_ID As Integer, ByRef Module_ID As Integer) As Integer
    Declare Function APS_set_board_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BOD_Param_No As Integer, ByVal BOD_Param As Integer) As Integer
    Declare Function APS_get_board_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BOD_Param_No As Integer, ByRef BOD_Param As Integer) As Integer
    Declare Function APS_set_axis_param Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal AXS_Param_No As Integer, ByVal AXS_Param As Integer) As Integer
    Declare Function APS_get_axis_param Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal AXS_Param_No As Integer, ByRef AXS_Param As Integer) As Integer
    Declare Function APS_get_system_timer Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef SysTimer As Integer) As Integer
    Declare Function APS_get_device_info Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Info_No As Integer, ByRef Info As Integer) As Integer
    Declare Function APS_get_card_name Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef CardName As Integer) As Integer
    Declare Function APS_disable_device Lib "APS168x64.dll" (ByVal DeviceName As Integer) As Integer
    Declare Function APS_get_first_axisId Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef StartAxisID As Integer, ByRef TotalAxisNum As Integer) As Integer
    Declare Function APS_set_security_key Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal OldPassword As Integer, ByVal NewPassword As Integer) As Integer
    Declare Function APS_check_security_key Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Password As Integer) As Integer
    Declare Function APS_reset_security_key Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_load_param_from_file Lib "APS168x64.dll" (ByVal pXMLFile As String) As Integer

    'Control driver mode [Only for PCI-8254/8]
    Declare Function APS_get_curr_sys_ctrl_mode Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Mode As Integer) As Integer

    '  Flash function [Only for PCI-8253/56, PCI-8392 Lib "APS168x64.dll" (H)]
    Declare Function APS_save_parameter_to_flash Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_load_parameter_from_flash Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_load_parameter_from_default Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    '
    '  SSCNET-3 functions [Only for PCI-8392 Lib "APS168x64.dll" (H)]
    Declare Function APS_start_sscnet Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef AxisFound_InBits As Integer) As Integer
    Declare Function APS_stop_sscnet Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_get_sscnet_servo_param Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Para_No1 As Integer, ByRef Para_Dat1 As Integer, ByVal Para_No2 As Integer, ByRef Para_Dat2 As Integer) As Integer
    Declare Function APS_set_sscnet_servo_param Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Para_No1 As Integer, ByVal Para_Dat1 As Integer, ByVal Para_No2 As Integer, ByVal Para_Dat2 As Integer) As Integer
    Declare Function APS_get_sscnet_servo_alarm Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Alarm_No As Integer, ByRef Alarm_Detail As Integer) As Integer
    Declare Function APS_reset_sscnet_servo_alarm Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_save_sscnet_servo_param Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_get_sscnet_servo_abs_position Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Cyc_Cnt As Integer, ByRef Res_Cnt As Integer) As Integer
    Declare Function APS_save_sscnet_servo_abs_position Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_load_sscnet_servo_abs_position Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Abs_Option As Integer, ByRef Cyc_Cnt As Integer, ByRef Res_Cnt As Integer) As Integer
    Declare Function APS_get_sscnet_link_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Link_Status As Integer) As Integer
    Declare Function APS_set_sscnet_servo_monitor_src Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Mon_No As Integer, ByVal Mon_Src As Integer) As Integer
    Declare Function APS_get_sscnet_servo_monitor_src Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Mon_No As Integer, ByRef Mon_Src As Integer) As Integer
    Declare Function APS_get_sscnet_servo_monitor_data Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Arr_Size As Integer, ByRef Data_Arr As Integer) As Integer
    Declare Function APS_set_sscnet_control_mode Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Mode As Integer) As Integer
    '
    'PWM function
    Declare Function APS_set_pwm_width Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByVal Width As Integer) As Integer
    Declare Function APS_get_pwm_width Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByRef Width As Integer) As Integer
    Declare Function APS_set_pwm_frequency Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByVal Frequency As Integer) As Integer
    Declare Function APS_get_pwm_frequency Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByRef Frequency As Integer) As Integer
    Declare Function APS_set_pwm_on Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PWM_Ch As Integer, ByVal PWM_On As Integer) As Integer

    'Simultaneous move [Only for MNET series and 8392]
    Declare Function APS_set_relative_simultaneous_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByRef Axis_ID_Array As Integer, ByRef Distance_Array As Integer, ByRef Max_Speed_Array As Integer) As Integer
    Declare Function APS_set_absolute_simultaneous_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByRef Axis_ID_Array As Integer, ByRef Position_Array As Integer, ByRef Max_Speed_Array As Integer) As Integer
    Declare Function APS_start_simultaneous_move Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_stop_simultaneous_move Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_set_velocity_simultaneous_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByRef Axis_ID_Array As Integer, ByRef Max_Speed_Array As Integer) As Integer
    Declare Function APS_release_simultaneous_move Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_emg_stop_simultaneous_move Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer


    'Override functions [Only for MNET series]
    Declare Function APS_speed_override Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal MaxSpeed As Integer) As Integer

    'Only for MNET-1XMO/MNET-4XMO
    Declare Function APS_relative_move_ovrd Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Distance As Integer, ByVal Max_Speed As Integer) As Integer
    Declare Function APS_absolute_move_ovrd Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Position As Integer, ByVal Max_Speed As Integer) As Integer

    'VAO function(Laser function)
    Declare Function APS_set_vao_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Param_No As Integer, ByVal Param_Val As Integer) As Integer
    Declare Function APS_get_vao_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Param_No As Integer, ByRef Param_Val As Integer) As Integer
    Declare Function APS_set_vao_table Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Table_No As Integer, ByVal MinVelocity As Integer, ByVal VelInterval As Integer, ByVal TotalPoints As Integer, ByVal MappingDataArray() As Integer) As Integer
    Declare Function APS_switch_vao_table Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Table_No As Integer) As Integer
    Declare Function APS_start_vao Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Output_Ch As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_get_vao_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Status As Integer) As Integer
    Declare Function APS_check_vao_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Table_No As Integer, ByRef Status As Integer) As Integer
    Declare Function APS_set_vao_param_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Table_No As Integer, ByRef VaoData As VAO_DATA) As Integer
    Declare Function APS_get_vao_param_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Table_No As Integer, ByRef VaoData As VAO_DATA) As Integer

    '  Motion IO & motion status
    Declare Function APS_get_command Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef CommandCnt As Integer) As Integer
    Declare Function APS_set_command Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal CommandCnt As Integer) As Integer
    Declare Function APS_motion_status Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_motion_io_status Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_set_servo_on Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Servo_On As Integer) As Integer
    Declare Function APS_get_position Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Position As Integer) As Integer
    Declare Function APS_set_position Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Position As Integer) As Integer
    Declare Function APS_get_command_velocity Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Velocity As Integer) As Integer
    Declare Function APS_get_feedback_velocity Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Velocity As Integer) As Integer
    Declare Function APS_get_error_position Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Err_Pos As Integer) As Integer
    Declare Function APS_get_target_position Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Targ_Pos As Integer) As Integer
    '
    '  Single axis motion
    Declare Function APS_relative_move Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Distance As Integer, ByVal Max_Speed As Integer) As Integer
    Declare Function APS_absolute_move Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Position As Integer, ByVal Max_Speed As Integer) As Integer
    Declare Function APS_velocity_move Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Max_Speed As Integer) As Integer
    Declare Function APS_home_move Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_stop_move Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_emg_stop Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_relative_move2 Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Distance As Integer, ByVal Start_Speed As Integer, ByVal Max_Speed As Integer, ByVal End_Speed As Integer, ByVal Acc_Rate As Integer, ByVal Dec_Rate As Integer) As Integer
    Declare Function APS_absolute_move2 Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Position As Integer, ByVal Start_Speed As Integer, ByVal Max_Speed As Integer, ByVal End_Speed As Integer, ByVal Acc_Rate As Integer, ByVal Dec_Rate As Integer) As Integer
    Declare Function APS_home_move2 Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Driection As Integer, ByVal Acc As Integer, ByVal Start_Speed As Integer, ByVal Max_Speed As Integer, ByVal ORG_Speed As Integer) As Integer
    Declare Function APS_home_escape Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer

    ' JOG functions [Only for PCI-8392, PCI-8253/56]
    Declare Function APS_set_jog_param Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef pStr_Jog As JOG_DATA, ByVal Mask As Integer) As Integer
    Declare Function APS_get_jog_param Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef pStr_Jog As JOG_DATA) As Integer
    Declare Function APS_jog_mode_switch Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Turn_No As Integer) As Integer
    Declare Function APS_jog_start Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal STA_On As Integer) As Integer
    '
    '  Interpolation
    Declare Function APS_absolute_linear_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Position_Array() As Integer, ByVal Max_Linear_Speed As Integer) As Integer
    Declare Function APS_relative_linear_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Distance_Array() As Integer, ByVal Max_Linear_Speed As Integer) As Integer
    Declare Function APS_absolute_arc_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Center_Pos_Array() As Integer, ByVal Max_Arc_Speed As Integer, ByVal Angle As Integer) As Integer
    Declare Function APS_relative_arc_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Center_Offset_Array() As Integer, ByVal Max_Arc_Speed As Integer, ByVal Angle As Integer) As Integer
    '
    ' Helical interpolation [Only for PCI-8392, PCI-8253/56]
    Declare Function APS_absolute_helix_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Center_Pos_Array() As Integer, ByVal Max_Arc_Speed As Integer, ByVal Pitch As Integer, ByVal TotalHeight As Integer, ByVal CwOrCcw As Integer) As Integer
    Declare Function APS_relative_helix_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Center_PosOffset_Array() As Integer, ByVal Max_Arc_Speed As Integer, ByVal Pitch As Integer, ByVal TotalHeight As Integer, ByVal CwOrCcw As Integer) As Integer

    ' Circular interpolation( Support 2D and 3D ) [Only for PCI-8392, PCI-8253/56]
    Declare Function APS_absolute_arc_move_3pe Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Pass_Pos_Array() As Integer, ByVal End_Pos_Array() As Integer, ByVal Max_Arc_Speed As Integer) As Integer
    Declare Function APS_relative_arc_move_3pe Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Pass_PosOffset_Array() As Integer, ByVal End_PosOffset_Array() As Integer, ByVal Max_Arc_Speed As Integer) As Integer

    '  Interrupt functions
    Declare Function APS_int_enable Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_set_int_factor Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Item_No As Integer, ByVal Factor_No As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_get_int_factor Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Item_No As Integer, ByVal Factor_No As Integer, ByRef Enable As Integer) As Integer
    Declare Function APS_set_int_factorH Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Item_No As Integer, ByVal Factor_No As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_int_no_to_handle Lib "APS168x64.dll" (ByVal Int_No As Integer) As Integer
    Declare Function APS_set_field_bus_int_factor_di Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal bitsOfCheck As Integer) As Integer
    Declare Function APS_get_field_bus_int_factor_di Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, bitsOfCheck As Integer) As Integer

    '//[Only for PCI-7856 motion interrupt]
    Declare Function APS_set_field_bus_int_factor_motion Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Factor_No As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_get_field_bus_int_factor_motion Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Factor_No As Integer, Enable As Integer) As Integer
    Declare Function APS_set_field_bus_int_factor_error Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Factor_No As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_get_field_bus_int_factor_error Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Factor_No As Integer, Enable As Integer) As Integer
    Declare Function APS_reset_field_bus_int_motion Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_wait_field_bus_error_int_motion Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Time_Out As Integer) As Integer



    '
    Declare Function APS_wait_single_int Lib "APS168x64.dll" (ByVal Int_No As Integer, ByVal Time_Out As Integer) As Integer
    Declare Function APS_wait_multiple_int Lib "APS168x64.dll" (ByVal Int_Count As Integer, ByVal Int_No_Array() As Integer, ByVal Wait_All As Integer, ByVal Time_Out As Integer) As Integer
    Declare Function APS_reset_int Lib "APS168x64.dll" (ByVal Int_No As Integer) As Integer
    Declare Function APS_set_int Lib "APS168x64.dll" (ByVal Int_No As Integer) As Integer
    ' [Only for PCI-8154/58]
    Declare Function APS_wait_error_int Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Item_No As Integer, ByVal Time_Out As Integer) As Integer
    '
    '  Sampling functions [Only for PCI-8392, PCI-8253/56]
    Declare Function APS_set_sampling_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal ParaNum As Integer, ByVal ParaDat As Integer) As Integer
    Declare Function APS_get_sampling_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal ParaNum As Integer, ByRef ParaDat As Integer) As Integer
    Declare Function APS_wait_trigger_sampling Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Length As Integer, ByVal PreTrgLen As Integer, ByVal TimeOutMs As Integer, ByRef DataArr As STR_SAMP_DATA_4CH) As Integer
    Declare Function APS_wait_trigger_sampling_async Lib "APS168x64.dll" (ByVal Board_ID As Object, ByVal Length As Integer, ByVal PreTrgLen As Integer, ByVal TimeOutMs As Integer, ByRef DataArr As STR_SAMP_DATA_4CH) As Integer
    Declare Function APS_get_sampling_count Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef SampCnt As Integer) As Integer
    Declare Function APS_stop_wait_sampling Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_auto_sampling Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal StartStop As Integer) As Integer
    Declare Function APS_get_sampling_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, Length As Integer, ByRef DataArr As STR_SAMP_DATA_4CH, Status As Integer) As Integer
    '
    ' DIO & AIO
    Declare Function APS_write_d_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByVal DO_Data As Integer) As Integer
    Declare Function APS_read_d_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByRef DO_Data As Integer) As Integer
    Declare Function APS_read_d_input Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DI_Group As Integer, ByRef DI_Data As Integer) As Integer
    Declare Function APS_write_d_channel_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByVal Ch_No As Integer, ByVal DO_Data As Integer) As Integer
    Declare Function APS_read_d_channel_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal DO_Group As Integer, ByVal Ch_No As Integer, ByRef DO_Data As Integer) As Integer
    '
    Declare Function APS_read_a_input_value Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByRef Convert_Data As Double) As Integer
    Declare Function APS_read_a_input_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByRef Raw_Data As Integer) As Integer
    Declare Function APS_write_a_output_value Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByVal Convert_Data As Double) As Integer
    Declare Function APS_write_a_output_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByVal Raw_Data As Integer) As Integer
    '
    ' Point table move
    Declare Function APS_set_point_table Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Index As Integer, ByRef Point As POINT_DATA) As Integer
    Declare Function APS_get_point_table Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Index As Integer, ByRef Point As POINT_DATA) As Integer
    Declare Function APS_get_running_point_index Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Index As Integer) As Integer
    Declare Function APS_get_start_point_index Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Index As Integer) As Integer
    Declare Function APS_get_end_point_index Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Index As Integer) As Integer
    Declare Function APS_set_table_move_pause Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Pause_en As Integer) As Integer
    Declare Function APS_set_table_move_repeat Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Repeat_en As Integer) As Integer
    Declare Function APS_point_table_move Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal StartIndex As Integer, ByVal EndIndex As Integer) As Integer

    Declare Function APS_set_point_tableEx Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Index As Integer, ByRef Point As PNT_DATA) As Integer
    Declare Function APS_set_point_table_4DL Lib "APS168x64.dll" (ByRef Axis_ID_Array As Integer, ByVal Index As Integer, ByRef Point As PNT_DATA_4DL) As Integer
    Declare Function APS_set_point_tableEx_2D Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Axis_ID_2 As Integer, ByVal Index As Integer, ByRef Point As PNT_DATA_2D) As Integer


    '//Point table + IO - Pause / Resume
    Declare Function APS_set_table_move_ex_pause Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer
    Declare Function APS_set_table_move_ex_rollback Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Max_Speed As Integer) As Integer
    Declare Function APS_set_table_move_ex_resume Lib "APS168x64.dll" (ByVal Axis_ID As Integer) As Integer

    '//Only for PCI-8392 to replace APS_set_point_table & APS_get_point_table
    Declare Function APS_set_point_table_ex Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Index As Integer, ByRef Point As POINT_DATA_EX) As Integer
    Declare Function APS_get_point_table_ex Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Index As Integer, ByRef Point As POINT_DATA_EX) As Integer

    '//Point table Feeder (Only for PCI-825x)
    Declare Function APS_set_feeder_group Lib "APS168x64.dll" (ByVal GroupId As Integer, ByVal Dimension As Integer, Axis_ID_Array As Integer) As Integer
    Declare Function APS_get_feeder_group Lib "APS168x64.dll" (ByVal GroupId As Integer, Dimension As Integer, Axis_ID_Array As Integer) As Integer
    Declare Function APS_free_feeder_group Lib "APS168x64.dll" (ByVal GroupId As Integer) As Integer
    Declare Function APS_reset_feeder_buffer Lib "APS168x64.dll" (ByVal GroupId As Integer) As Integer
    Declare Function APS_set_feeder_point_2D Lib "APS168x64.dll" (ByVal GroupId As Integer, ByRef PtArray As PNT_DATA_2D, ByVal Size As Integer, ByVal LastFlag As Integer) As Integer
    Declare Function APS_set_feeder_point_2D_ex Lib "APS168x64.dll" (ByVal GroupId As Integer, ByRef PtArray As PNT_DATA_2D_F64, ByVal Size As Integer, ByVal LastFlag As Integer) As Integer
    Declare Function APS_start_feeder_move Lib "APS168x64.dll" (ByVal GroupId As Integer) As Integer
    Declare Function APS_get_feeder_status Lib "APS168x64.dll" (ByVal GroupId As Integer, State As Integer, ErrCode As Integer) As Integer
    Declare Function APS_get_feeder_running_index Lib "APS168x64.dll" (ByVal GroupId As Integer, Index As Integer) As Integer
    Declare Function APS_get_feeder_feed_index Lib "APS168x64.dll" (ByVal GroupId As Integer, Index As Integer) As Integer
    Declare Function APS_set_feeder_ex_pause Lib "APS168x64.dll" (ByVal GroupId As Integer) As Integer
    Declare Function APS_set_feeder_ex_rollback Lib "APS168x64.dll" (ByVal GroupId As Integer, ByVal Max_Speed As Integer) As Integer
    Declare Function APS_set_feeder_ex_resume Lib "APS168x64.dll" (ByVal GroupId As Integer) As Integer
    Declare Function APS_set_feeder_cfg_acc_type Lib "APS168x64.dll" (ByVal GroupId As Integer, ByVal type As Integer) As Integer



    '
    'Point table move2
    Declare Function APS_set_point_table_mode2 Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Mode As Integer) As Integer
    Declare Function APS_set_point_table2 Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Index As Integer, ByRef Point As POINT_DATA2) As Integer
    Declare Function APS_point_table_continuous_move2 Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer) As Integer
    Declare Function APS_point_table_single_move2 Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Index As Integer) As Integer
    Declare Function APS_get_running_point_index2 Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Index As Integer) As Integer
    Declare Function APS_point_table_status2 Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Status As Integer) As Integer

    'Point table Only for HSL-4XMO
    Declare Function APS_set_point_table3 Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal Index As Integer, ByRef Point As POINT_DATA3) As Integer
    Declare Function APS_point_table_move3 Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal StartIndex As Integer, ByVal EndIndex As Integer) As Integer
    Declare Function APS_set_point_table_param3 Lib "APS168x64.dll" (ByVal FirstAxid As Integer, ByVal ParaNum As Integer, ByVal ParaDat As Integer) As Integer

    'Gantry functions. [Only for PCI-8392, PCI-8253/56]
    Declare Function APS_set_gantry_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal GroupNum As Integer, ByVal ParaNum As Integer, ByVal ParaDat As Integer) As Integer
    Declare Function APS_get_gantry_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal GroupNum As Integer, ByVal ParaNum As Integer, ByRef ParaDat As Integer) As Integer
    Declare Function APS_set_gantry_axis Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal GroupNum As Integer, ByVal Master_Axis_ID As Integer, ByVal Slave_Axis_ID As Integer) As Integer
    Declare Function APS_get_gantry_axis Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal GroupNum As Integer, ByRef Master_Axis_ID As Integer, ByRef Slave_Axis_ID As Integer) As Integer
    Declare Function APS_get_gantry_error Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal GroupNum As Integer, ByRef GentryError As Integer) As Integer
    '
    'Digital filter functions. [Only for PCI-8253/56]
    Declare Function APS_set_filter_param Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Filter_paramNo As Integer, ByVal param_val As Integer) As Integer
    Declare Function APS_get_filter_param Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Filter_paramNo As Integer, ByRef param_val As Integer) As Integer
    Declare Function APS_manual_set_filter Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Coefficient As FILTER_COEF) As Integer
    Declare Function APS_get_filter_coef Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Coefficient As FILTER_COEF) As Integer

    ' Field bus master fucntions For PCI-8392(H)
    Declare Function APS_set_field_bus_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal BUS_Param_No As Integer, ByVal BUS_Param As Integer) As Integer
    Declare Function APS_get_field_bus_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal BUS_Param_No As Integer, ByRef BUS_Param As Integer) As Integer
    Declare Function APS_start_field_bus Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal Start_Axis_ID As Integer) As Integer
    Declare Function APS_stop_field_bus Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer) As Integer
    '
    Declare Function APS_get_field_bus_device_info Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Info_No As Integer, ByRef Info As Integer) As Integer
    Declare Function APS_get_field_bus_last_scan_info Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByRef Info_Array As Integer, ByVal Array_Size As Integer, ByRef Info_Count As Integer) As Integer
    Declare Function APS_get_field_bus_master_type Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByRef BUS_Type As Integer) As Integer
    Declare Function APS_get_field_bus_slave_type Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByRef MOD_Type As Integer) As Integer
    Declare Function APS_get_field_bus_slave_name Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByRef MOD_Name As Integer) As Integer
    Declare Function APS_get_field_bus_slave_first_axisno Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByRef AxisNo As Integer, ByRef TotalAxes As Integer) As Integer
    '
    ' Field bus slave general functions
    Declare Function APS_set_field_bus_slave_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByVal ParaNum As Integer, ByVal ParaDat As Integer) As Integer
    Declare Function APS_get_field_bus_slave_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByVal ParaNum As Integer, ByRef ParaDat As Integer) As Integer
    Declare Function APS_get_slave_connect_quality Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByRef Sts_data As Integer) As Integer
    Declare Function APS_get_slave_online_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByRef Live As Integer) As Integer
    '
    ' Field bus DIO slave fucntions For PCI-8392(H)
    Declare Function APS_set_field_bus_d_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal DO_Value As Integer) As Integer
    Declare Function APS_get_field_bus_d_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByRef DO_Value As Integer) As Integer
    Declare Function APS_get_field_bus_d_input Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByRef DI_Value As Integer) As Integer
    Declare Function APS_set_field_bus_d_channel_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByVal DO_Value As Integer) As Integer
    Declare Function APS_get_field_bus_d_channel_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByRef DO_Value As Integer) As Integer
    Declare Function APS_get_field_bus_d_channel_input Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByRef DI_Value As Integer) As Integer
    '
    ' Field bus AIO slave function
    Declare Function APS_set_field_bus_a_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByVal AO_Value As Double) As Integer
    Declare Function APS_get_field_bus_a_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByRef AO_Value As Double) As Integer
    Declare Function APS_get_field_bus_a_input Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByRef AI_Value As Double) As Integer
    Declare Function APS_get_field_bus_a_input_plc Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByRef AI_Value As Double, ByVal RunStep As Double) As Integer





    'Field bus Comparing trigger functions
    Declare Function APS_set_field_bus_trigger_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Param_No As Integer, ByVal param_val As Integer) As Integer
    Declare Function APS_get_field_bus_trigger_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Param_No As Integer, ByRef param_val As Integer) As Integer
    Declare Function APS_set_field_bus_trigger_linear Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal LCmpCh As Integer, ByVal StartPoint As Integer, ByVal RepeatTimes As Integer, ByVal Interval As Integer) As Integer
    Declare Function APS_set_field_bus_trigger_table Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal TCmpCh As Integer, ByVal DataArr() As Integer, ByVal ArraySize As Integer) As Integer
    Declare Function APS_set_field_bus_trigger_manual Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal TrgCh As Integer) As Integer
    Declare Function APS_set_field_bus_trigger_manual_s Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal TrgChInBit As Object) As Integer
    Declare Function APS_get_field_bus_trigger_table_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal TCmpCh As Integer, ByRef CmpVal As Integer) As Integer
    Declare Function APS_get_field_bus_trigger_linear_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal LCmpCh As Integer, ByRef CmpVal As Integer) As Integer
    Declare Function APS_get_field_bus_trigger_count Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal TrgCh As Integer, ByRef TrgCnt As Integer) As Integer
    Declare Function APS_reset_field_bus_trigger_count Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal TrgCh As Integer) As Integer
    Declare Function APS_get_field_bus_linear_cmp_remain_count Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal LCmpCh As Integer, ByRef Cnt As Integer) As Integer
    Declare Function APS_get_field_bus_table_cmp_remain_count Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal TCmpCh As Integer, ByRef Cnt As Integer) As Integer
    Declare Function APS_get_field_bus_encoder Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal EncCh As Integer, ByRef EncCnt As Integer) As Integer
    Declare Function APS_set_field_bus_encoder Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal EncCh As Integer, ByVal EncCnt As Integer) As Integer

    '  Comparing trigger functions
    Declare Function APS_set_trigger_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Param_No As Integer, ByVal param_val As Integer) As Integer
    Declare Function APS_get_trigger_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Param_No As Integer, ByRef param_val As Integer) As Integer
    Declare Function APS_set_trigger_linear Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal LCmpCh As Integer, ByVal StartPoint As Integer, ByVal RepeatTimes As Integer, ByVal Interval As Integer) As Integer
    Declare Function APS_set_trigger_table Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TCmpCh As Integer, ByVal DataArr() As Integer, ByVal ArraySize As Integer) As Integer
    Declare Function APS_set_trigger_manual Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer) As Integer
    Declare Function APS_set_trigger_manual_s Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgChInBit As Integer) As Integer
    Declare Function APS_get_trigger_table_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TCmpCh As Integer, ByRef CmpVal As Integer) As Integer
    Declare Function APS_get_trigger_linear_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal LCmpCh As Integer, ByRef CmpVal As Integer) As Integer
    Declare Function APS_get_trigger_count Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByRef TrgCnt As Integer) As Integer
    Declare Function APS_reset_trigger_count Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer) As Integer
    Declare Function APS_enable_trigger_fifo_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_get_trigger_fifo_cmp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByRef CmpVal As Integer) As Integer
    Declare Function APS_get_trigger_fifo_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FCmpCh As Integer, FifoSts As Integer) As Integer
    Declare Function APS_set_trigger_fifo_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FCmpCh As Integer, ByRef DataArr As Integer, ByVal ArraySize As Integer, ByVal ShiftFlag As Integer) As Integer
    Declare Function APS_set_trigger_encoder_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByVal TrgCnt As Integer) As Integer
    Declare Function APS_get_trigger_encoder_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByRef TrgCnt As Integer) As Integer


    '
    '  Pulser counter function
    Declare Function APS_get_pulser_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Counter As Integer) As Integer
    Declare Function APS_set_pulser_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Counter As Integer) As Integer
    '
    '  Reserved functions
    Declare Function APS_field_bus_slave_set_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByVal ParaNum As Integer, ByVal ParaDat As Integer) As Integer
    Declare Function APS_field_bus_slave_get_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByVal ParaNum As Integer, ByRef ParaDat As Integer) As Integer
    '
    Declare Function APS_field_bus_d_set_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal DO_Value As Integer) As Integer
    Declare Function APS_field_bus_d_get_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByRef DO_Value As Integer) As Integer
    Declare Function APS_field_bus_d_get_input Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByRef DI_Value As Integer) As Integer
    '
    Declare Function APS_field_bus_A_set_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByVal AO_Value As Double) As Integer
    Declare Function APS_field_bus_A_get_output Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByRef AO_Value As Double) As Integer
    Declare Function APS_field_bus_A_get_input Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByRef AI_Value As Double) As Integer


    Declare Function APS_field_bus_A_set_output_plc Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByVal AO_Value As Double, ByVal RunStep As Integer) As Integer
    Declare Function APS_field_bus_A_get_input_plc Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal BUS_No As Integer, ByVal MOD_No As Integer, ByVal Ch_No As Integer, ByRef AI_Value As Double, ByVal RunStep As Integer) As Integer



    ' Dpac Function
    Declare Function APS_rescan_CF Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_get_battery_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Battery_status As Integer) As Integer
    '
    ' DPAC Display & Display Button
    Declare Function APS_get_display_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal displayDigit As Integer, ByRef displayIndex As Integer) As Integer
    Declare Function APS_set_display_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal displayDigit As Integer, ByVal displayIndex As Integer) As Integer
    Declare Function APS_get_button_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef buttonstatus As Integer) As Integer
    '
    ' nv RAM funciton
    Declare Function APS_set_nv_ram Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal RamNo As Integer, ByVal DataWidth As Integer, ByVal Offset As Integer, ByVal data As Integer) As Integer
    Declare Function APS_get_nv_ram Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal RamNo As Integer, ByVal DataWidth As Integer, ByVal Offset As Integer, ByRef data As Integer) As Integer
    Declare Function APS_clear_nv_ram Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal RamNo As Integer) As Integer

    'Timer funciton
    Declare Function APS_get_timer_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TmrCh As Integer, ByRef Cnt As Integer) As Integer
    Declare Function APS_set_timer_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TmrCh As Integer, ByVal Cnt As Integer) As Integer
    Declare Function APS_start_timer Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TrgCh As Integer, ByVal Start As Integer) As Integer

    'Virtual board settings  [Only for PCI-8254/8]
    Declare Function APS_register_virtual_board Lib "APS168x64.dll" (ByVal VirCardIndex As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_get_virtual_board_info Lib "APS168x64.dll" (ByVal VirCardIndex As Integer, ByRef Enable As Integer) As Integer
    Declare Function APS_register_virtual_board_ex Lib "APS168x64.dll" (ByVal VirCardIndex As Integer, ByVal Count As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_get_virtual_board_info_ex Lib "APS168x64.dll" (ByVal VirCardIndex As Integer, ByRef Count As Integer, ByRef Enable As Integer) As Integer
    'Parameters setting by float [Only for PCI-8254/8]
    Declare Function APS_set_axis_param_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal AXS_Param_No As Integer, ByVal AXS_Param As Double) As Integer
    Declare Function APS_get_axis_param_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal AXS_Param_No As Integer, ByRef AXS_Param As Double) As Integer
    'Control driver mode [Only for PCI-8254/8]
    Declare Function APS_get_eep_curr_drv_ctrl_mode Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef ModeInBit As Integer) As Integer

    ' Monitor functions by float [Only for PCI-8254/8]
    Declare Function APS_get_command_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Command As Double) As Integer
    Declare Function APS_set_command_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Command As Double) As Integer
    Declare Function APS_get_position_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Position As Double) As Integer
    Declare Function APS_set_position_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Position As Double) As Integer
    Declare Function APS_get_command_velocity_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Velocity As Double) As Integer
    Declare Function APS_get_target_position_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Targ_Pos As Double) As Integer
    Declare Function APS_get_error_position_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Err_Pos As Double) As Integer
    Declare Function APS_get_feedback_velocity_f Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Velocity As Double) As Integer
    'Motion queue status [Only for PCI-8254/8]
    Declare Function APS_get_mq_free_space Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Space As Integer) As Integer
    Declare Function APS_get_mq_usage Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Usage As Integer) As Integer
    'Motion Stop Code [Only for PCI-8254/8]
    Declare Function APS_get_stop_code Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Code As Integer) As Integer

    ' Sampling functions extension[Only for PCI-82548 for up to 8 channel]
    Declare Function APS_set_sampling_param_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Param As SAMP_PARAM) As Integer
    Declare Function APS_get_sampling_param_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Param As SAMP_PARAM) As Integer
    Declare Function APS_wait_trigger_sampling_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Length As Integer, ByVal PreTrgLen As Integer, ByVal TimeOutMs As Integer, ByRef DataArr As STR_SAMP_DATA_8CH) As Integer
    Declare Function APS_wait_trigger_sampling_async_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Length As Integer, ByVal PreTrgLen As Integer, ByVal TimeOutMs As Integer, ByRef DataArr As STR_SAMP_DATA_8CH) As Integer
    Declare Function APS_get_sampling_data_ex Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Length As Integer, ByRef DataArr As STR_SAMP_DATA_8CH, ByRef Status As Integer) As Integer

    'New AIO [Only for PCI-82548]
    Declare Function APS_read_a_output_value Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Channel_No As Integer, ByRef Convert_Data As Double) As Integer

    'New Interface
    Declare Function APS_ptp Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal APS_Option As Integer, ByVal Position As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_ptp_v Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal APS_Option As Integer, ByVal Position As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_ptp_all Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal APS_Option As Integer, ByVal Position As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_vel Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal APS_Option As Integer, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_vel_all Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal APS_Option As Integer, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_line Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal PositionArray() As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_line_v Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal PositionArray() As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_line_all Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal PositionArray() As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc2_ca Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc2_ca_v Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc2_ca_all Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc2_ce Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc2_ce_v Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc2_ce_all Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc3_ca Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc3_ca_v Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc3_ca_all Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc3_ce Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc3_ce_v Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_arc3_ce_all Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_spiral_ca Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByVal DeltaH As Double, ByVal FinalR As Double, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_spiral_ca_v Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByVal DeltaH As Double, ByVal FinalR As Double, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_spiral_ca_all Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal Angle As Double, ByVal DeltaH As Double, ByVal FinalR As Double, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_spiral_ce Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_spiral_ce_v Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vm As Double, ByRef Wait As ASYNCALL) As Integer
    Declare Function APS_spiral_ce_all Lib "APS168x64.dll" (ByVal Axis_ID_Array() As Integer, ByVal APS_Option As Integer, ByVal CenterArray() As Double, ByVal NormalArray() As Double, ByVal EndArray() As Double, ByVal Dir As Short, ByRef TransPara As Double, ByVal Vs As Double, ByVal Vm As Double, ByVal Ve As Double, ByVal Acc As Double, ByVal Dec As Double, ByVal SFac As Double, ByRef Wait As ASYNCALL) As Integer





    'Point table Feeder  Lib "APS168x64.dll" (Only for PCI-8254/8)
    Declare Function APS_pt_dwell Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTDWL, ByRef Status As PTSTS) As Integer
    Declare Function APS_pt_line Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTLINE, ByRef Status As PTSTS) As Integer
    Declare Function APS_pt_arc2_ca Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA2CA, ByRef Status As PTSTS) As Integer
    Declare Function APS_pt_arc2_ce Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA2CE, ByRef Status As PTSTS) As Integer
    Declare Function APS_pt_arc3_ca Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA3CA, ByRef Status As PTSTS) As Integer
    Declare Function APS_pt_arc3_ce Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTA3CE, ByRef Status As PTSTS) As Integer
    Declare Function APS_pt_spiral_ca Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTHCA, ByRef Status As PTSTS) As Integer
    Declare Function APS_pt_spiral_ce Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Prof As PTHCE, ByRef Status As PTSTS) As Integer

    'enable & disable
    Declare Function APS_pt_enable Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Dimension As Integer, ByVal AxisArr() As Integer) As Integer
    Declare Function APS_pt_disable Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
    Declare Function APS_get_pt_info Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Info As PTINFO) As Integer
    Declare Function APS_pt_set_vs Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Vs As Double) As Integer
    Declare Function APS_pt_get_vs Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Vs As Double) As Integer
    Declare Function APS_pt_start Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
    Declare Function APS_pt_stop Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
    Declare Function APS_get_pt_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef Status As PTSTS) As Integer
    Declare Function APS_reset_pt_buffer Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
    Declare Function APS_pt_roll_back Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Max_Speed As Double) As Integer
    Declare Function APS_pt_get_error Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByRef ErrCode As Integer) As Integer

    'Cmd buffer setting
    Declare Function APS_pt_ext_set_do_ch Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Channel As Integer, ByVal OnOff As Integer) As Integer
    Declare Function APS_pt_ext_set_table_no Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal CtrlNo As Integer, ByVal TableNo As Integer) As Integer



    'Profile buffer setting
    Declare Function APS_pt_set_absolute Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
    Declare Function APS_pt_set_relative Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
    Declare Function APS_pt_set_trans_buffered Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
    Declare Function APS_pt_set_trans_inp Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer) As Integer
    Declare Function APS_pt_set_trans_blend_dec Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Bp As Double) As Integer
    Declare Function APS_pt_set_trans_blend_dist Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Bp As Double) As Integer
    Declare Function APS_pt_set_trans_blend_pcnt Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Bp As Double) As Integer
    Declare Function APS_pt_set_acc Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Acc As Double) As Integer
    Declare Function APS_pt_set_dec Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Dec As Double) As Integer
    Declare Function APS_pt_set_acc_dec Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal AccDec As Double) As Integer
    Declare Function APS_pt_set_s Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Sf As Double) As Integer
    Declare Function APS_pt_set_vm Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Vm As Double) As Integer
    Declare Function APS_pt_set_ve Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal PtbId As Integer, ByVal Ve As Double) As Integer

    'Program download - APS
    Declare Function APS_load_vmc_program Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef pFile As String, ByVal Password As Integer) As Integer
    Declare Function APS_save_vmc_program Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef pFile As String, ByVal Password As Integer) As Integer
    Declare Function APS_load_amc_program Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef pFile As String, ByVal Password As Integer) As Integer
    Declare Function APS_save_amc_program Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef pFile As String, ByVal Password As Integer) As Integer
    Declare Function APS_set_task_mode Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByVal Mode As Byte, ByVal LastIP As UShort) As Integer
    Declare Function APS_get_task_mode Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef Mode As Byte, ByRef LastIP As UShort) As Integer
    Declare Function APS_start_task Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByVal CtrlCmd As Integer) As Integer
    Declare Function APS_get_task_info Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TaskNum As Integer, ByRef Info As TSK_INFO) As Integer
    Declare Function APS_get_task_msg Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef QueueSts As UShort, ByRef ActualSize As UShort, ByRef CharArr As Byte) As Integer

    '//Latch functins
    Declare Function APS_get_encoder Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Encoder As Integer) As Integer
    Declare Function APS_get_latch_counter Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Src As Integer, ByRef Counter As Integer) As Integer
    Declare Function APS_get_latch_event Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Src As Integer, ByRef ENT As Integer) As Integer

    'Raw command counter [Only for PCI-8254/8]
    Declare Function APS_get_command_counter Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Counter As Integer) As Integer

    '//Watch dog timer
    Declare Function APS_wdt_start Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByVal TimeOut As Integer) As Integer
    Declare Function APS_wdt_get_timeout_period Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByRef TimeOut As Integer) As Integer
    Declare Function APS_wdt_reset_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer) As Integer
    Declare Function APS_wdt_get_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByRef Counter As Integer) As Integer
    Declare Function APS_wdt_set_action_event Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByVal EventByBit As Integer) As Integer
    Declare Function APS_wdt_get_action_event Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal TimerNo As Integer, ByRef EventByBit As Integer) As Integer

    'Multi-axes simultaneuos move start/stop
    Declare Function APS_move_trigger Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer) As Integer
    Declare Function APS_stop_move_multi Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer) As Integer
    Declare Function APS_emg_stop_multi Lib "APS168x64.dll" (ByVal Dimension As Integer, ByVal Axis_ID_Array() As Integer) As Integer

    'Gear/Gantry function
    Declare Function APS_start_gear Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByVal Mode As Integer) As Integer
    Declare Function APS_get_gear_status Lib "APS168x64.dll" (ByVal Axis_ID As Integer, ByRef Status As Integer) As Integer

    '//Latch Function: for latching multi-points
    Declare Function APS_set_ltc_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal CntNum As Integer, ByVal CntValue As Integer) As Integer
    Declare Function APS_get_ltc_counter Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal CntNum As Integer, ByRef CntValue As Integer) As Integer
    Declare Function APS_set_ltc_fifo_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByVal Param_No As Integer, ByVal Param_No As Integer) As Integer
    Declare Function APS_get_ltc_fifo_param Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByVal Param_No As Integer, ByRef Param_No As Integer) As Integer
    Declare Function APS_manual_latch Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal LatchSignalInBits As Integer) As Integer
    Declare Function APS_enable_ltc_fifo Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_reset_ltc_fifo Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer) As Integer
    Declare Function APS_get_ltc_fifo_data Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByRef Data As Integer) As Integer
    Declare Function APS_get_ltc_fifo_usage Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByRef Usage As Integer) As Integer
    Declare Function APS_get_ltc_fifo_free_space Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByRef FreeSpace As Integer) As Integer
    Declare Function APS_get_ltc_fifo_status Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal FLtcCh As Integer, ByRef Status As Integer) As Integer

    '//For Single latch for PCI8154/58/MNET-4XMO-(C)
    Declare Function APS_manual_latch2 Lib "APS168x64.dll" (ByVal Board_ID As Integer) As Integer
    Declare Function APS_get_latch_data2 Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal LatchNum As Integer, ByRef LatchData As Integer) As Integer
    Declare Function APS_set_backlash_en Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByVal Enable As Integer) As Integer
    Declare Function APS_get_backlash_en Lib "APS168x64.dll" (ByVal Board_ID As Integer, ByRef Enable As Integer) As Integer




End Module