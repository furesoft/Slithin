FROM alpine:3.6
WORKDIR /

ARG ROOT_PASSWORD
ENV ROOT_PASSWORD=${ROOT_PASSWORD:-remarkable}

RUN apk add --update --no-cache openssh
RUN mkdir /var/run/sshd
RUN echo 'root:remarkable' | chpasswd


RUN echo 'PasswordAuthentication yes' >> /etc/ssh/sshd_config
RUN echo 'PermitRootLogin yes' >> /etc/ssh/sshd_config


RUN echo "alias systemctl=\"\"" >> ~/.bashrc && \
    echo "alias restart=\"\"" >> ~/.bashrc && \
    echo "alias xochitl=\"\"" >> ~/.bashrc

RUN echo "Welcome to the Remarkable Emulator" >> /etc/motd

RUN mv -v home /home/

RUN rm -rf /etc/ssh/ssh_host_rsa_key /etc/ssh/ssh_host_dsa_key && \
    ssh-keygen -A	

EXPOSE 22
CMD ["/usr/sbin/sshd", "-D"]