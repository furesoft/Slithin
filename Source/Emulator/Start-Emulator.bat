docker build --no-cache -t remarkable-emulator .
docker run -it -p 22:22 --name remarkable-emulator --rm remarkable-emulator

PAUSE