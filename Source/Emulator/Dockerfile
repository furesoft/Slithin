FROM apline:3.6
WORKDIR /home/remarkeble

ARG ROOT_PASSWORD
ENV ROOT_PASSWORD=${ROOT_PASSWORD:-remarkeble}

RUN apk add --update --no-cache openssh && \
    sed -e 's/#PermitRootLogin .*/PermitRootLogin yes/' /etc/ssh/sshd_config && \
    echo $ROOT_PASSWORD | passwd root --stdin && \
    echo "alias systemctl=\"\"" >> ~/.bashrc && \
    echo "alias restart=\"\"" >> ~/.bashrc && \
    echo "alias xochitl=\"\"" >> ~/.bashrc

EXPOSE 22
CMD ["ssh"]