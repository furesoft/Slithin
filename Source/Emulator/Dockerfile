FROM alpine:3.6
WORKDIR /

ARG ROOT_PASSWORD
ENV ROOT_PASSWORD=${ROOT_PASSWORD:-remarkable}

RUN apk add --update --no-cache openssh && \
    mkdir /var/run/sshd && \
    echo 'PasswordAuthentication yes' >> /etc/ssh/sshd_config && \
    echo 'PermitRootLogin yes' >> /etc/ssh/sshd_config && \
    echo "alias systemctl=\"\"" >> ~/.bashrc && \
    echo "alias restart=\"\"" >> ~/.bashrc && \
    echo "alias xochitl=\"\"" >> ~/.bashrc && \
    echo "Welcome to the Remarkable Emulator" >> /etc/motd && \
    rm -rf /etc/ssh/ssh_host_rsa_key /etc/ssh/ssh_host_dsa_key && \
    ssh-keygen -A

COPY remarkable_data/* /

RUN echo "root:${ROOT_PASSWORD}" | chpasswd

EXPOSE 22
CMD ["/usr/sbin/sshd", "-D"]