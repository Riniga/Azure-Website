const express = require('express')
const cors = require('cors')
const bodyParser = require('body-parser')
const readline = require("readline");

const app = express()
app.use(cors())
app.use(bodyParser.json())
const port = 4000

const consolereader = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});


app.listen(port, () => {
    console.log(`Node JS Push Server for the Service Worker Web Site.`)
    console.log(`~~~~~~~~~~~~~~~~~~~~*\\o/~~~~~/\\*~~~~~~~~~~~~~~~~~~~~`)
    console.log(`Server is listening at http://localhost:${port}`)
    ListenForCommand();
})

const memoryDb = { subscriptions: [] }
async function saveToDatabase(subscription) {
    memoryDb.subscriptions.push(subscription);
}

function ListenForCommand() {
    consolereader.question("Command: ", function (value) {
        
        if (value == "quit") {
            process.exit(1)
            return
        }
        else {
            console.log("Sending message: " + value);
            memoryDb.subscriptions.forEach(subscription => {
                sendNotification(subscription, value)
                console.log("Sent sent to subscriber ");
            });

            console.log("Sent to "+ memoryDb.subscriptions.length + " subscribers");
            
        }
        ListenForCommand();
    });
}

const webpush = require('web-push')
const vapidKeys =
{
    publicKey: 'BKhSHyPZOZiEhBfIvnLRosMKpWeprqHWXK5r7Pv650HYlOkpbn16-ri4tJubVNDvO7zhWSytqQhsh3ngsuv348M',
    privateKey: 'WCtNHUsiQ2pGSRRl06mgV1L0NKduw55E3WhTXb0hiRw',
}
webpush.setVapidDetails('mailto:rickard@nisses-gagner.se', vapidKeys.publicKey, vapidKeys.privateKey);

function sendNotification(subscription, dataToSend = '') {
    webpush.sendNotification(subscription, dataToSend)
}

app.post('/save-subscription', async (req, res) => {
    console.log("\r\nA new client subscribed\r\nCommand:");
    const subscription = req.body;
    await saveToDatabase(subscription);
    res.json({ message: 'success' });
})