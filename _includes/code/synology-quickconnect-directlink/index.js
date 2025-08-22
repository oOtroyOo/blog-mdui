const http = require('http');
const { URL } = require('url');

let qc_map = {}

/**
 * @param {http.IncomingMessage} req 
 * @param {http.ServerResponse} res 
 */
async function handle(req, res) {
    let qc_id = req.url.split('/', 2)[1]
    if (!qc_id) {
        res.writeHead(400, { 'Content-Type': 'text/plain;charset=utf-8' });
        res.end('Bad Request');
        return
    }
    if (!qc_map[qc_id]) {

        const raw = JSON.stringify([
            {
                "version": 1,
                "command": "get_server_info",
                "stop_when_error": false,
                "stop_when_success": false,
                "id": "mainapp_http",
                "serverID": qc_id,
                "is_gofile": false,
                "path": ""
            }
        ]);

        /**  @type {RequestInit} */
        const requestOptions = {
            method: "POST",
            body: raw
        };

        const result = await fetch("https://global.quickconnect.cn/Serv.php", requestOptions)
            .then((response) => response.json())
            .catch((error) => console.error(error));
        for (data of result) {
            if (data.service && data.service.relay_ip) {
                qc_map[qc_id] = {
                    relay_ip: data.service.relay_ip,
                    relay_port: data.service.relay_port
                }
                break
            }
        }
    }

    if (qc_map[qc_id]) {
        let url = new URL(req.url.substring(qc_id.length + 1), `http://${qc_map[qc_id].relay_ip}:${qc_map[qc_id].relay_port}`);
        res.writeHead(308, { 'Location': url.toString() });
    } else {
        res.writeHead(404, { 'Content-Type': 'text/plain;charset=utf-8' });
        res.end('Not Found');
    }
}

const server = http.createServer(async (req, res) => {

    try {
        await handle(req, res);
        res.end();
    } catch (err) {
        // 捕获异常并返回给前端
        res.writeHead(500, { 'Content-Type': 'application/json;charset=utf-8' });
        // let msg = `${err.message}\n${err.stack.replace(/^/gm, '  ')}\n`;
        let msg = err.stack
        res.end(msg);
    }
});
server.on('error', function (e) {
    console.error(e);
});

server.listen(9000, () => {
    console.log('server is running on port 9000');
});