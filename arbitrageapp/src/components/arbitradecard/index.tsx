import React, { FC } from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import { ArbitrageDto } from '../../apis/arbitrage';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';


interface ArbitrageCardProps {
  currentData: ArbitrageDto;
  history: ArbitrageDto[];
  title: string
}

export const ArbitrageCard: FC<ArbitrageCardProps> = ({ currentData,history, title }) => {
  return (
    <Card>
      <CardContent>
        <Typography variant="h5" component="h2" gutterBottom>
          {title} Arbitrage
        </Typography>
        <Typography variant="body1" color="textSecondary">
          USD ASK: ${currentData.bitstampAskPrice}
        </Typography>
        <Typography variant="body1" color="textSecondary">
          ZAR BID: R{currentData.valrBidPrice}
        </Typography>
        <Typography variant="body1" color="textSecondary">
          Exchange Rate (USD to ZAR): {currentData.exchangeRate}
        </Typography>
        <Typography variant="body1" color="textSecondary">
          Arbitrage Ratio: {currentData.arbitrageRatio}
        </Typography>
        <ResponsiveContainer width="100%" height={300}>
          <LineChart data={history}>
            <XAxis dataKey="timestamp" />
            <YAxis />
            <CartesianGrid strokeDasharray="3 3" />
            <Tooltip />
            <Legend />
            <Line type="monotone" dataKey="arbitrageRatio" stroke="#8884d8" />
          </LineChart>
        </ResponsiveContainer>
      </CardContent>
    </Card>
  );
};

export default ArbitrageCard;