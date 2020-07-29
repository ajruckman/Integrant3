const generate = require('@elemental-design/coloralgorithm');
const readline = require('readline');

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
});

rl.on('line', function (line) {
    if (line === "!end") rl.close();

    let args = line.split(' ');

    const input = {
        specs: {
            // Number of colors
            steps: parseInt(args[0]),
            // Hue Start Value (0 - 359)
            hue_start: parseInt(args[1]),
            // Hue End Value (0 - 359)
            hue_end: parseInt(args[2]),
            // Hue Curve (See Curves Section)
            hue_curve: args[3],
            // Saturation Start Value (0 - 100)
            sat_start: parseInt(args[4]),
            // Saturation End Value (0 - 100)
            sat_end: parseInt(args[5]),
            // Saturation Curve (See Curves Section)
            sat_curve: args[6],
            // Saturation Rate (0 - 200)
            sat_rate: parseInt(args[7]),
            // Luminosity Start Value (0 - 100)
            lum_start: parseInt(args[8]),
            // Luminosity End Value (0 - 100)
            lum_end: parseInt(args[9]),
            // Luminosity Curve (See Curves Section)
            lum_curve: args[10],
            // Modifier Scale
            // Every generated color gets a modifier (label) that
            // indicates its lightness. A value of 10 results in
            // two-digit modifiers. The lightest color will be 0 (e.g. Red 0)
            // and the darkest color will be 100 (e.g. Red 100), given
            // that you generate 11 colors
            modifier: parseInt(args[11]),
        },
    };

    const palette = generate(input);

    console.log(JSON.stringify(palette));

}).on('close', function () {
    console.log("Closing");
    process.exit(0);
});

// const input = {
//     specs: {
//         // Number of colors
//         steps: parseInt(process.argv[2]),
//         // Hue Start Value (0 - 359)
//         hue_start: parseInt(process.argv[3]),
//         // Hue End Value (0 - 359)
//         hue_end: parseInt(process.argv[4]),
//         // Hue Curve (See Curves Section)
//         hue_curve: process.argv[5],
//         // Saturation Start Value (0 - 100)
//         sat_start: parseInt(process.argv[6]),
//         // Saturation End Value (0 - 100)
//         sat_end: parseInt(process.argv[7]),
//         // Saturation Curve (See Curves Section)
//         sat_curve: process.argv[8],
//         // Saturation Rate (0 - 200)
//         sat_rate: parseInt(process.argv[9]),
//         // Luminosity Start Value (0 - 100)
//         lum_start: parseInt(process.argv[10]),
//         // Luminosity End Value (0 - 100)
//         lum_end: parseInt(process.argv[11]),
//         // Luminosity Curve (See Curves Section)
//         lum_curve: process.argv[12],
//         // Modifier Scale
//         // Every generated color gets a modifier (label) that
//         // indicates its lightness. A value of 10 results in
//         // two-digit modifiers. The lightest color will be 0 (e.g. Red 0)
//         // and the darkest color will be 100 (e.g. Red 100), given
//         // that you generate 11 colors
//         modifier: parseInt(process.argv[13]),
//     },
// };
//
// const palette = generate(input);
//
// console.log(palette);
