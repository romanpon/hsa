**Select all**
```
mysql> select count(*) from hsa8.users;

+----------+
| count(*) |
+----------+
| 10982723 |
+----------+
1 row in set (21.27 sec)
```


**Select without index**
```
mysql> select count(*) from hsa8.users where `birthDate` = '2000-01-01';

+----------+
| count(*) |
+----------+
|      100 |
+----------+
1 row in set (12.19 sec)
```

**Select with BTree index**
```
mysql> CREATE INDEX birthDate_index USING BTREE ON hsa8.users ( birthDate );
Query OK, 0 rows affected (43.19 sec)
```

```
mysql> select count(*) from hsa8.users where `birthDate` = '2000-01-01';
+----------+
| count(*) |
+----------+
|      100 |
+----------+
1 row in set (0.05 sec)

mysql> select count(*) from hsa8.users where `birthDate` > '2000-01-01';
+----------+
| count(*) |
+----------+
|   717220 |
+----------+
1 row in set (0.40 sec)

mysql> select count(*) from hsa8.users where `birthDate` < '2000-01-01';
+----------+
| count(*) |
+----------+
| 10265403 |
+----------+
1 row in set (5.44 sec)
```

```
mysql> DROP INDEX birthDate_index ON hsa8.users;
Query OK, 0 rows affected (0.12 sec)
```

**Select with Hash index**
```
mysql> CREATE INDEX birthDate_index USING HASH ON hsa8.users ( birthDate );
Query OK, 0 rows affected, 1 warning (55.18 sec)
```

```
mysql> select count(*) from hsa8.users where `birthDate` = '2000-01-01';
+----------+
| count(*) |
+----------+
|      100 |
+----------+
1 row in set (0.00 sec)

mysql> select count(*) from hsa8.users where `birthDate` > '2000-01-01';
+----------+
| count(*) |
+----------+
|   717220 |
+----------+
1 row in set (0.29 sec)

mysql> select count(*) from hsa8.users where `birthDate` < '2000-01-01';
+----------+
| count(*) |
+----------+
| 10265403 |
+----------+
1 row in set (3.83 sec)
```

**Insert**
```
mysql> SET GLOBAL innodb_flush_log_at_trx_commit=0;

roman@ubuntu:~$ siege -c10 -t45S 'http://192.168.88.237:5000/data POST'
** SIEGE 4.0.4
** Preparing 10 concurrent users for battle.
The server is now under siege...
Lifting the server siege...
Transactions:		       11421 hits
Availability:		      100.00 %
Elapsed time:		       44.97 secs
Data transferred:	        0.00 MB
Response time:		        0.04 secs
Transaction rate:	      253.97 trans/sec
Throughput:		        0.00 MB/sec
Concurrency:		        9.46
Successful transactions:       11421
Failed transactions:	           0
Longest transaction:	        0.26
Shortest transaction:	        0.01

roman@ubuntu:~$ siege -c50 -t45S 'http://192.168.88.237:5000/data POST'
** SIEGE 4.0.4
** Preparing 50 concurrent users for battle.
The server is now under siege...
Lifting the server siege...
Transactions:		       11535 hits
Availability:		      100.00 %
Elapsed time:		       44.20 secs
Data transferred:	        0.00 MB
Response time:		        0.17 secs
Transaction rate:	      260.97 trans/sec
Throughput:		        0.00 MB/sec
Concurrency:		       44.42
Successful transactions:       11535
Failed transactions:	           0
Longest transaction:	        1.12
Shortest transaction:	        0.01

roman@ubuntu:~$ siege -c100 -t45S 'http://192.168.88.237:5000/data POST'
** SIEGE 4.0.4
** Preparing 100 concurrent users for battle.
The server is now under siege...
Lifting the server siege...
Transactions:		       11588 hits
Availability:		      100.00 %
Elapsed time:		       45.12 secs
Data transferred:	        0.00 MB
Response time:		        0.33 secs
Transaction rate:	      256.83 trans/sec
Throughput:		        0.00 MB/sec
Concurrency:		       85.17
Successful transactions:       11594
Failed transactions:	           0
Longest transaction:	        3.78
Shortest transaction:	        0.01
```

```
mysql> SET GLOBAL innodb_flush_log_at_trx_commit=1;

roman@ubuntu:~$ siege -c10 -t45S 'http://192.168.88.237:5000/data POST'
** SIEGE 4.0.4
** Preparing 10 concurrent users for battle.
The server is now under siege...
Lifting the server siege...
Transactions:		        9967 hits
Availability:		      100.00 %
Elapsed time:		       44.81 secs
Data transferred:	        0.00 MB
Response time:		        0.04 secs
Transaction rate:	      222.43 trans/sec
Throughput:		        0.00 MB/sec
Concurrency:		        9.63
Successful transactions:        9967
Failed transactions:	           0
Longest transaction:	        0.16
Shortest transaction:	        0.01

roman@ubuntu:~$ siege -c50 -t45S 'http://192.168.88.237:5000/data POST'
** SIEGE 4.0.4
** Preparing 50 concurrent users for battle.
The server is now under siege...
Lifting the server siege...
Transactions:		       10389 hits
Availability:		      100.00 %
Elapsed time:		       45.07 secs
Data transferred:	        0.00 MB
Response time:		        0.21 secs
Transaction rate:	      230.51 trans/sec
Throughput:		        0.00 MB/sec
Concurrency:		       47.37
Successful transactions:       10389
Failed transactions:	           0
Longest transaction:	        1.03
Shortest transaction:	        0.02

roman@ubuntu:~$ siege -c100 -t45S 'http://192.168.88.237:5000/data POST'
** SIEGE 4.0.4
** Preparing 100 concurrent users for battle.
The server is now under siege...
Lifting the server siege...
Transactions:		       10347 hits
Availability:		      100.00 %
Elapsed time:		       44.51 secs
Data transferred:	        0.00 MB
Response time:		        0.39 secs
Transaction rate:	      232.46 trans/sec
Throughput:		        0.00 MB/sec
Concurrency:		       91.33
Successful transactions:       10347
Failed transactions:	           0
Longest transaction:	        2.75
Shortest transaction:	        0.02
```

```
mysql> SET GLOBAL innodb_flush_log_at_trx_commit=2;

roman@ubuntu:~$ siege -c10 -t45S 'http://192.168.88.237:5000/data POST'
** SIEGE 4.0.4
** Preparing 10 concurrent users for battle.
The server is now under siege...
Lifting the server siege...
Transactions:		       11079 hits
Availability:		      100.00 %
Elapsed time:		       44.96 secs
Data transferred:	        0.00 MB
Response time:		        0.04 secs
Transaction rate:	      246.42 trans/sec
Throughput:		        0.00 MB/sec
Concurrency:		        9.55
Successful transactions:       11079
Failed transactions:	           0
Longest transaction:	        0.17
Shortest transaction:	        0.01

roman@ubuntu:~$ siege -c50 -t45S 'http://192.168.88.237:5000/data POST'
** SIEGE 4.0.4
** Preparing 50 concurrent users for battle.
The server is now under siege...
Lifting the server siege...
Transactions:		        9959 hits
Availability:		      100.00 %
Elapsed time:		       44.98 secs
Data transferred:	        0.00 MB
Response time:		        0.21 secs
Transaction rate:	      221.41 trans/sec
Throughput:		        0.00 MB/sec
Concurrency:		       46.99
Successful transactions:        9959
Failed transactions:	           0
Longest transaction:	        0.96
Shortest transaction:	        0.02

roman@ubuntu:~$ siege -c100 -t45S 'http://192.168.88.237:5000/data POST'
** SIEGE 4.0.4
** Preparing 100 concurrent users for battle.
The server is now under siege...
Lifting the server siege...
Transactions:		       10432 hits
Availability:		      100.00 %
Elapsed time:		       45.69 secs
Data transferred:	        0.00 MB
Response time:		        0.41 secs
Transaction rate:	      228.32 trans/sec
Throughput:		        0.00 MB/sec
Concurrency:		       92.81
Successful transactions:       10434
Failed transactions:	           0
Longest transaction:	        3.24
Shortest transaction:	        0.02
```



