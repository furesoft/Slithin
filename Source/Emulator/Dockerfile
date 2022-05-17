FROM alpine:3.6
WORKDIR /home/remarkeble

ARG ROOT_PASSWORD
ENV ROOT_PASSWORD=${ROOT_PASSWORD:-remarkeble}

RUN apk add --update --no-cache openssh
RUN mkdir /var/run/sshd
RUN echo 'root:mypassword' | chpasswd


RUN echo 'PasswordAuthentication yes' >> /etc/ssh/sshd_config


RUN echo "alias systemctl=\"\"" >> ~/.bashrc && \
    echo "alias restart=\"\"" >> ~/.bashrc && \
    echo "alias xochitl=\"\"" >> ~/.bashrc
	

EXPOSE 22
CMD ["/usr/sbin/sshd", "-D"]