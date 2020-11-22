# cipherc [WIP]

An english-like bytes data process/cipher DSL.

e.g.

```bash
# transform data format
cipherc hex 61626364 print base64

# hash data
cipherc sm3 txt "to be hash" print hex

# output random 32bit
cipherc rand 32 print hex

# generate sm2 key pair then sign data
cipherc sm2 gen \
    sign from txt data "secret content" \
        to hex data

# sm4 enc and dec data
cipherc sm4 \
    set key from bin file "1.key" \
    set iv from hex data "000000" \
    enc ecb from txt data "abcdef" \
        to var enc_result \
    dec ecb from var enc_result \
        to base64 data

# variable and multiple expression
cipherc \
    sm2 set key from hex data "123456" \
        get pk to var x \
    then \
    sm3 from var x

# recursive expression
cipherc sm3 \
    sm2 \
        set key from hex data "123456" \
        get pk to out
```

## Grammar

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
        | print PrintFormat         // contains no space
        | printf PrintFormat        // print readable
        | to file <Path>
        | to var <VAR>

DataPrimary -> hex <HEX> | bin <BIN> | base64 <BASE64> | url <URL>
            | txt <UTF8> | var <VAR> | rand <N-bytes> | file <PATH>
            | pipe txt          // pipe input is text
            | pipe file         // pipe input is file path

PrintFormat  -> hex | bin | base64 | url | txt | ascii | auto
EncodeFormat -> hex | bin | base64 | url
DecodeFormat -> hex | bin | base64 | url | pem
HashOperator -> sm3 | md5 | sha1 | sha256 | sha384 | sha512 | sha3

// Statement

Statement ->
          ObjectSentence
        | print PrintFormat DataExpression

ObjectSentence ->
        | Object get <KEY>
        | Object set <KEY> Data
        | Object ObjectAction <PARAM> Data

Asym -> sm2 | rsa1024
Sym  -> sm4 | aes
Pack -> x509 | p10
Object -> Asym | Sym | Pack
ObjectAction -> enc | dec | sign | check
```
