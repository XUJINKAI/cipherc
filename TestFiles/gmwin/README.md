由Windows版本gmssl生成

```cmd
gmssl.exe crl2pkcs7 certfile cert.pem -out cert.p7b.pem -config pki.conf
gmssl.exe pkcs8 -topk8 -inform PEM -in sm2prikey.pem -nocrypt -out sm2prikey.p8.pem
```
