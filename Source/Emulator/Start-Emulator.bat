docker build -t remarkable-emulator .
docker run -it -p 22:22 --rm -v C:\Users\chris\Desktop\re\:/re remarkable-emulator

PAUSE