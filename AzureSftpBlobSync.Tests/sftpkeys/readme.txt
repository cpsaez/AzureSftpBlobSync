With this public key you can setup with docker an sftp server ready to be connected with the publickey.
Docker command:

(replace {PathOfThePublicKey} with the directory of this readme and where the publickey.txt file is stored)

docker run -v {PathOfThePublicKey}\publickey.txt:/home/foo/.ssh/keys/id_rsa.pub:ro -p 2223:22 -d atmoz/sftp foo::1001

