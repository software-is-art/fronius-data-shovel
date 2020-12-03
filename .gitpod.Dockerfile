FROM gitpod/workspace-full

RUN wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    sudo dpkg -i packages-microsoft-prod.deb

RUN sudo apt-get update && \
    sudo apt-get install -y apt-transport-https && \
    sudo apt-get update && \
    sudo apt-get install -y dotnet-sdk-5.0 && \
    sudo apt-get install -y dotnet-sdk-3.1

RUN dotnet tool install -g Amazon.Lambda.Tools
RUN curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" && \
    unzip awscliv2.zip && \
    sudo ./aws/install
