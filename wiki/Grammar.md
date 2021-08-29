# Grammar

```c
Block -> Sentence { then Sentence }
Sentence ->   DataExpression
            | var <VAR> is DataExpression
            | vars
            | help

// Expression

DataExpression -> DataFactor { concat DataFactor }      // +

// 对DataPrimary附加DataOperator运算
// 先向前计算，再向后计算
DataFactor ->  PostfixData
PostfixData -> PrefixData { DataOperator }
PrefixData ->  DataOperator PrefixData | DataPrimary

DataOperator ->
        | encode EncodeFormat
        | decode DecodeFormat
        | HashOperator
        | repeat <times>            // *
        | sub <Nstart> <Nlength>
        | print PrintFormat
        | to file <Path>
        | to var <VAR>

DataPrimary -> hex <HEX> | bin <BIN> | base64 <BASE64> | url <URL>
            | txt <UTF8> | var <VAR> | rand <N-bytes> | file <PATH>
            | pipe txt          // pipe input is text
            | pipe file         // pipe input is file path
            | <PATH>            // == file <PATH>
            | <URL>             // == url <URL>, starts with http(s)://
            | 0x<HEX>           // == hex <HEX>
            | 0b<BIN>           // == bin <BIN>

PrintFormat  -> hex | bin | base64 | url | txt | ascii | auto
              | hexf | binf | base64f
EncodeFormat -> hex | bin | base64 | url
DecodeFormat -> hex | bin | base64 | url
HashOperator -> sm3 | md5 | sha1 | sha256 | sha384 | sha512 | sha3

// WIP...

ObjectSentence ->
        DataParser DataExpression

ObjectSentence ->
        | Object get <KEY>
        | Object set <KEY> Data
        | Object ObjectAction <PARAM> Data

Object -> x509 | pem | der
        | sm2 | rsa1024
        | sm4 | aes

ObjectAction -> load | get | set
              | sign | verify
              | enc | dec
```
