FROM alpine:3.6
WORKDIR /home/remarkeble

ARG ROOT_PASSWORD
ENV ROOT_PASSWORD=${ROOT_PASSWORD:-remarkeble}

RUN apk add --update --no-cache openssh && \
    sed -e 's/#PermitRootLogin .*/PermitRootLogin yes/' /etc/ssh/sshd_config && \
    passwd root -d ${ROOT_PASSWORD} && \
    echo "alias systemctl=\"\"" >> ~/.bashrc && \
    echo "alias restart=\"\"" >> ~/.bashrc && \
    echo "alias xochitl=\"\"" >> ~/.bashrc
RUN rm -rf /etc/ssh/ssh_host_rsa_key /etc/ssh/ssh_host_dsa_key && \
    ssh-keygen -A

EXPOSE 22
CMD ["/usr/sbin/sshd", "-D"]