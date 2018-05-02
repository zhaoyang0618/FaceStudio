#ifndef _HQVT_FACE_H	
#define _HQVT_FACE_H

#ifdef __cplusplus
extern "C" {
#endif

/*------------------------------------------------------------------------------*/
//����ʶ��
/*------------------------------------------------------------------------------*/
typedef struct _FACE_DETECT_PARAM_S
{
	DWORD				dwSize;
	BYTE				bEnable;		//�Ƿ�����������ץ�Ĺ���
	BYTE				bPicDispFaceArea;//ץ����Ƭ�Ƿ����������
	BYTE				bSnapMode;		//ץ��ģʽ��0x1:���뿪��ץ��,0x2:ʵʱץ��,0x4:���ץ��
	BYTE				bSnapThreshold;	//ץ�·�ֵ��range 0-10,default 1��(5.0.36֮��汾����)
	AREA_RECT_S			stDetectArea;	//���ܼ������ ����ο�704*576
	float				fSensitivity;	//��������� rang 0-1,default 0.4
	BYTE                bPreviewFaceArea;//Ԥ���Ƿ���������� 0 ������ 1 ����������ͷ�� 2 ������������������ͷ��
	BYTE                bShootPicType;  //ץ���ϴ� 0x1�ϴ�ԭͼ 0x2 �ϴ�����ͼƬ 0x3�ϴ�ԭͼ��������ͼ
	BYTE				bPicSendSvr;    //ͼƬ�ϴ�������������
	BYTE				bPicSendFtp;    //ͼƬ�ϴ���FTP	
	URL_PORT_S          stFaceSvr;      //����ͼƬ��������ַ
	BYTE				bFaceAreaAE;	//�Ƿ������������Զ��ع� 0 ��ͨģʽ 1 �Զ�ģʽ
	BYTE				bFaceROI;		//�Ƿ�����������Ȥ�������
	WORD                wFaceMinPixel;  //���������С���� 32~672��Ĭ��ֵ96��Ӧ:(720p default: 64*64 max: 450*450)
	DWORD               dwRealShootTrackFr;//ʵʱץ�������֡��
	float               fPeriodShootIntv;//���ץ��ʱ�䣬��λ��
	WORD                wMaxSnapPicCnt;  //���ץ����: ����ÿ������Ŀ�����ץ��ͼƬ����,0������
	BYTE                bMaxFaceCntLimit;//��֡���������,0������    
	BYTE                bFaceCreateDist; //���������ƶ����� 1-30��ֵԽ�����������ٶ�Խ��
	WORD				wSnapFaceWidth;	 //����ץͼ��С,0����ԭʼ���
	WORD				wSnapFaceHeight; //����ץͼ��С,0����ԭʼ�߶�
	SEG_TIME_S          stSegTime[2];    //�������ʱ���
	//--------------------------------------------------------------------------------------------------------------
	BYTE                bySendFtpPathType;//�ϴ�FTP·�� 0 ���豸IDΪ��Ŀ¼ 1 �Ե�ǰ����Ϊ��Ŀ¼
	BYTE                byUpgradeMode;  //������ʽ 0: ZMODEM 1��STREAM
	BYTE                byFacePicQulity;//����ͼץ��ͼƬ��������
	BYTE                byConnCenter;   // ������������ƽ̨
	URL_PORT_S          stCenterAddr;   // ���������IP�˿�
	char                szConnectID[MAX_ID_LEN];//����ID
	float               fFacePicSceneRatio; //����ͼƬ�ܱ�����ϵ�� 
	BYTE				byFaceExpAutoModeStrength;//�����ع�����Ӧģʽǿ��ֵ:0~30, Ŀǰ������(5.0.25֮��汾����)
	BYTE				byFaceBright;		//��������	:0~30, Ŀǰ������(5.0.25֮��汾����)
	BYTE                byRes1[2];
	DWORD               dwUpgradeBaudrate;  //����������: 115200/230400/460800/576000/921600/1000000
	BYTE				byRes2[14];
	BYTE                bfaceAlgoMonitor;//�Ƿ������㷨��� 0 ������ 1 ����㷨�Ƿ����� 2 ����㷨�Ƿ������������
	BYTE                bProcSnapMode;   //ץͼ����ʽ 0: �첽 1: ͬ��(���Բ�����Release�汾��ʹ���첽��ʽ�����ô˲���?) ͬ����ʽ:ֱ���ڽ��������̴߳��������ع�/����ROI/����ץͼ...
										 //�첽��ʽ: �ڽ��������߳̽����������ع�/����ROI���½��̴߳�������ץͼ����ֹ��Ϊ����ץͼ������ʱ���������ع�/����ROI/������ͺ�˴�����ʱ
	BYTE                bySendSvrProtocol;//����ͼƬ�ϴ��������ϴ���ʽ 0: �Զ���Э���ϴ� 1��HTTP�ϴ�
	BYTE                byRes3[3];  //
	DWORD               dwPicHttpPostId;
	char                szPicHttpPostPath[100];
	BYTE				bStartSnapQulity;   //����ץ��������ֵ����Χ0-100����������������������>=���õ�ץ��������ֵʱ��ʼץ��
	BYTE                byRes4[627];
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	BYTE                bySeparate;
	BYTE                byEnableBodySnap;//������ץ������汾��Ч
	WORD                wBodyPicSceneRatioTop;//������ץ������汾��Ч
	WORD                wBodyPicSceneRatioBotton;//������ץ������汾��Ч              
	WORD                wBodyPicSceneRatioLeft;//������ץ������汾��Ч
	WORD                wBodyPicSceneRatioRight;//������ץ������汾��Ч
	WORD                wBodyPicSnapTrackFr;//������ץ������汾��Ч
	BYTE				byRes5[256];
}FACE_DETECT_PARAM_S;

/*
typedef struct _FACE_DETECT_PARAM_S
{
	DWORD				dwSize;
	BYTE				bEnable;		//�Ƿ�����������ץ�Ĺ���
	BYTE				bPicDispFaceArea;//ץ����Ƭ�Ƿ����������
	BYTE				bSnapMode;		//ץ��ģʽ��0x1:���뿪��ץ��,0x2:ʵʱץ��,0x4:���ץ��
	BYTE				bSnapThreshold;	//ץ�·�ֵ��range 1-10,default 4
	AREA_RECT_S			stDetectArea;	//���ܼ������ ����ο�704*576
	float				fSensitivity;	//��������� rang 0-1,default 0.4
	BYTE                bPreviewFaceArea;//Ԥ���Ƿ���������� 0 ������ 1 ����������ͷ�� 2 ������������������ͷ��
	BYTE                bShootPicType;  //ץ���ϴ� 0x1�ϴ�ԭͼ 0x2 �ϴ�����ͼƬ 0x3�ϴ�ԭͼ��������ͼ
	BYTE				bPicSendSvr;    //ͼƬ�ϴ�������������
	BYTE				bPicSendFtp;    //ͼƬ�ϴ���FTP	
	URL_PORT_S          stFaceSvr;      //������������ַ
	BYTE				bFaceAreaAE;	//�Ƿ������������Զ��ع�
	BYTE				bFaceROI;		//�Ƿ�����������Ȥ�������
	WORD                wFaceMinPixel;  //���������С���� 32~672��Ĭ��ֵ96��Ӧ:(720p default: 64*64 max: 450*450)
	DWORD               dwRealShootTrackFr;//ʵʱץ�������֡��
	float               fPeriodShootIntv;//���ץ��ʱ�䣬��λ��
	WORD                wMaxSnapPicCnt;  //���ץ����: ����ÿ������Ŀ�����ץ��ͼƬ����,0������
	BYTE                bMaxFaceCntLimit;//��֡���������,0������    
	BYTE                bFaceCreateDist; //���������ٶ� 1-5
	WORD				wSnapFaceWidth;	 //����ץͼ��С,0����ԭʼ���
	WORD				wSnapFaceHeight; //����ץͼ��С,0����ԭʼ�߶�
	SEG_TIME_S          stSegTime[2];
	//--------------------------------------------------------------------------------------------------------------
	BYTE                bySendFtpPathType;
	BYTE                byRes1[2];
	BYTE                byConnCenter;   // ������������ƽ̨
	URL_PORT_S          stCenterAddr;   // ���������IP�˿�
	char                szConnDeviceID[32];//����ID
	float               fFacePicSceneRatio;
	BYTE				byFaceExpAutoModeStrength;	 //�����ع�����Ӧģʽǿ��ֵ
	BYTE				byFaceBright;		//��������		
	BYTE				byRes2[20];
	BYTE                bfaceAlgoMonitor;//�Ƿ������㷨���
	BYTE                bProcSnapMode;   //ץͼ����ʽ 0: �첽 1: ͬ��(���Բ�����Release�汾��ʹ���첽��ʽ�����ô˲���?) ͬ����ʽ:ֱ���ڽ��������̴߳��������ع�/����ROI/����ץͼ...
	//�첽��ʽ: �ڽ��������߳̽����������ع�/����ROI���½��̴߳�������ץͼ����ֹ��Ϊ����ץͼ������ʱ���������ع�/����ROI/������ͺ�˴�����ʱ
}FACE_DETECT_PARAM_S;
*/
// typedef struct __ICE_FACE_RESULT_S
// {
//     DWORD       dwSize;						//�ϴ��������ܳ��ȣ�ĿǰΪ�ýṹ�峤��
// 	DWORD		dwTimeStamp;				//ʱ�����Ϣ,��������Ƶ֡ͬ�����ݲ�ͬ������Ч��
// 	INT			dwAlgoWidth;				//dwAlgoWidth��dwAlgoHeight���㷨�����ͼ���͸�
// 	INT			dwAlgoHeight;				//�ϱ�����������ο�����ϵ��Ϊ(dwAlgoWidth, dwAlgoHeight)
// 	INT 		s32TargetNum;				//!< ������Ŀ
// 	AREA_RECT_S astTargets[32];				//!< ��������
// 
// 	BYTE		byRes[236];				
// }ICE_FACE_RESULT_S; 

typedef struct __ICE_FACE_RESULT_S
{
	DWORD       dwSize;         // �ϴ��������ܳ��ȣ�ĿǰΪ�ýṹ�峤��
	DWORD		dwTimeStamp;    // ʱ�����Ϣ,��������Ƶ֡ͬ�����ݲ�ͬ������Ч��
	INT			dwAlgoWidth;	//dwAlgoWidth��dwAlgoHeight���㷨�����ͼ���͸�
	INT			dwAlgoHeight;   //�ϱ�����������ο�����ϵ��Ϊ(dwAlgoWidth, dwAlgoHeight)
	INT 		s32TargetNum;   //Ŀ����Ŀ������������ͷ��
	AREA_RECT_S astTargetArea[32]; //Ŀ��λ��
	BYTE        byTargetType[32];  //Ŀ������ 0: ���� 1: ͷ��
	DWORD       dwTargetId[32];    //Ŀ��ID�����ٺ�
	BYTE        byTargetQuality[32];//Ŀ��������������
	BYTE		byRes[44];
}ICE_FACE_RESULT_S; 	


typedef struct _ICEFACE_MSG_NOTIFY
{
	HANDLE					hLogonServer;
	DWORD					dwClientID;
	DWORD					dwServerIP;
	DWORD					dwServerPort;
	ICE_FACE_RESULT_S		stIceFaceResult;
}ICEFACE_MSG_NOTIFY;

//typedef struct _FACE_PIC_INFO_S
//{
//	DWORD				dwBgnFlag;			//0x12345678
//	DWORD				dwSize;				//�ṹ��С sizeof(FACE_PIC_INFO_S)	
//	DWORD				dwIPAddr;			//�豸IP
//	DWORD				dwDeviceID;			//�豸ID
//	long long			u64Time;			//ץͼ��ʱ��    
//	WORD				wImageWidth;		//ͼƬ���
//	WORD				wImageHeight;		//ͼƬ�߶�	
//	DWORD   			dwPicLen;
//	FACE_AREA_S			stTargets;			//��������
//	DWORD				stTrackID;			//���ٺ�
//	DWORD 				dwPicType;			//ͼƬ����	0 ���뿪��ץ�� 1 ʵʱץ��
//	char				dwReserved[204];	//����2		
//	DWORD				dwEndFlag;			//0x87654321
//}FACE_PIC_INFO_S; 

typedef struct _FACE_FEATURE_INFO
{
	DWORD   			dwDataLen;
}FACE_FEATURE_INFO;/*����������Ϣ*/

typedef struct __FACE_AREA_S
{
	WORD				wRoiLeft;
	WORD				wRoiTop;
	WORD				wRoiRight;
	WORD				wRoiBottom;
}FACE_AREA_S;

typedef struct _FACE_SNAP_PIC_INFO
{
	DWORD              dwPicLen;           //ͼƬ����
	UInt8               byCapType;          //ץ������	0x1 ���뿪��ץ�� 0x2 ʵʱץ�� 0x4 ���ץ��
	UInt8               byImageType;        //ͼƬ����	0x1 ����ͼ 0x2 ����ͼ 0x4 ����ͼ
	UInt8               byQulity;           //0~100
	UInt8               byEncoderType;      //Ŀǰ��ֵΪ0����jpegͼƬ
	long long           u64CapTime;         //ץͼ��ʱ��    
	WORD              wImageWidth;        //ͼƬ���
	WORD              wImageHeight;       //ͼƬ�߶�  
	FACE_AREA_S         stTgtFaceArea;      //��������
	WORD                wCapMsec;           //ץͼ��ʱ�䣬����
	UInt8               byRes[18];
}FACE_SNAP_PIC_INFO;/*ͼƬ��Ϣ*/


typedef struct _MYIP6ADDR
{
	UInt8		u6_addr8[16];
}IP6ADDR;

typedef struct _FACE_PICS_INFO_S
{
	DWORD              dwBgnFlag;          //0x12345678
	DWORD              dwSize;             //��Ϣ���� = wHeadSize + dwTotDataLen;Ϊһ��ͼƬ�������������ϢͨѶ�����ܳ���
	DWORD              dwIPAddr;           //�豸IP,��byIpAddrTypeΪ0ʱ��Ч
	DWORD              dwDeviceID;         //�豸ID
	DWORD              dwTrackID;          //�������ٺ�
	WORD               wVersion;           //Э��汾�ţ�ĿǰΪ1
	UInt8              byIpAddrType;       //�����IP��ַ����: 0: ipv4 1:ipv6
	UInt8               byRes1[1];
	WORD                wHeadSize;          //�ṹ��С sizeof(FACE_PICS_INFO_S)
	UInt8               byFeatureNum;       //�����������ݸ�����Ŀǰ��֧�֣�ֵΪ0
	UInt8               byPicNum;           //ͼƬ��
	DWORD              dwTotDataLen;       //ͼƬ������+�������������ܳ��ȣ�Ŀǰ�ݲ�֧�������������ݣ�ΪͼƬ�������ܳ���
	FACE_SNAP_PIC_INFO  stPics[2];          //ͼƬ��Ϣ
	FACE_FEATURE_INFO   stFeature[2];       //��������������Ϣ
	char				byRes2[84];	        //����2	
	DWORD				dwDevSendPicTimes;  //������ϴ�ͼƬ�������豸�����󣬵�һ���ϴ�ͼƬΪ0��ÿ���ϴ�ͼƬ������1
	IP6ADDR             stIpAddr6;          //�����ipv6��ַ����byIpAddrTypeΪ1ʱ��Ч����struct in6_addr�ṹ����
	long long           u64TgtCreateTime;   //����Ŀ������ʱ��
    DWORD				dwTgtSendPicTimes;  //��ǰץ��Ŀ���ϴ�ͼƬ������Ŀ���һ���ϴ�ͼƬΪ0����Ŀ��ÿ���ϴ�ͼƬ��������1
	DWORD              dwEndFlag;          //0x87654321
}FACE_PICS_INFO_S;


#ifdef __cplusplus
}
#endif

#endif

