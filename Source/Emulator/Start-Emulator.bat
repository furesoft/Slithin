docker build -t remarkable-emulator .
docker run -it -p 22:22 --rm -v %CD%\\remarkable_data:/re remarkable-emulator

PAUSE