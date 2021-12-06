# cipherc (WIP)

cipherc是一款密码算法命令行工具，支持国密算法(SM2、SM3、SM4)及常见国际算法，同时支持各种常见的数据格式解析及转换。

# 示例

- 生成一串16字节的随机数，打印其hex, base64格式，同时保存到文件中

```bash
rand 16 --dump hex,base64 --out rand.bin
# 输出:
# 78756A696E6B61692E6E65745F5F5F5F
# eHVqaW5rYWkubmV0X19fXw==
```

- 将base64格式数据转换为C语言字符串格式

```bash
base64 "Y2lwaGVyYw==" --dump hex.c
# 输出: \x63\x69\x70\x68\x65\x72\x63
```

- 计算SM3杂凑值，默认向屏幕打印其hex值

```bash
# [-f] 指示输入为文件名
sm3 -f <file-path>
# 否则默认输入为hex值
sm3 00aaFF
# 输出: 72E7CA83F1A17EAECE95BB01D3C859F8A86592C313971F714CFD97D1381A6DF0
```

- SM4解密文件，将结果转换为utf8字符串打印到屏幕上

```bash
sm4 -d -K 0123456789ABCDEFFEDCBA9876543210 --iv 0 -f <file-path> --dump utf8
```

[核心测试](ciphercTest/Core_Test.cs)

[使用说明](docs/usage.md)

## LICENSE

[MIT](LICENSE)
