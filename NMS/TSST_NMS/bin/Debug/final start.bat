start TSST_NMS.exe d1
timeout 1
start /min TSST_NMS.exe s1
timeout 1
start /min TSST_NMS.exe d2
timeout 1
start /min TSST_Cloud_v2.exe cloud_config.txt
timeout 1
start /min TSST_Node.exe 1 1
timeout 1
start /min TSST_Node.exe 1 2
timeout 1
start /min TSST_Node.exe 1 3
timeout 1
start /min TSST_Node.exe 2 4
timeout 1
start /min TSST_Node.exe 2 5
timeout 1
start /min TSST_Node.exe 2 6
timeout 1
start /min TSST_Node.exe 3 7
timeout 1
start /min TSST_Node.exe 3 8
timeout 1
start /min TSST_Node.exe 3 9
timeout 1
start TSST_Client.exe 1 1
timeout 1
start TSST_Client.exe 3 2