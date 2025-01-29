"use client";

import {
  ChartConfig,
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,
} from "@/components/ui/chart";
import * as React from "react";
import { Bar, BarChart, CartesianGrid, LabelList, XAxis } from "recharts";
import { CreditCard, DollarSign, Handshake, TrendingUp } from "lucide-react";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { useEffect, useState } from "react";
import { Avatar, AvatarFallback } from "@radix-ui/react-avatar";
import { Order } from "@/types/order";
import { Skeleton } from "@/components/ui/skeleton"; // Make sure to import Skeleton

const chartConfig = {
  desktop: {
    label: "Orders",
    color: "#2563eb",
  },
} satisfies ChartConfig;

const ALL_MONTHS = [
  "January",
  "February",
  "March",
  "April",
  "May",
  "June",
  "July",
  "August",
  "September",
  "October",
  "November",
  "December",
];

// Skeleton component for Revenue/Profits/Sales cards
const MetricCardSkeleton = () => (
  <Card className="rounded-xl bg-muted/50">
    <CardHeader className="flex flex-row items-center justify-between space-y-0">
      <Skeleton className="rounded-full h-4 w-24 mb-1" />
      <Skeleton className="rounded-full h-4 w-4" />
    </CardHeader>
    <CardContent>
      <Skeleton className="rounded-full h-8 w-36" />
    </CardContent>
  </Card>
);

const MetricThirdCardSkeleton = () => (
  <Card className="rounded-xl bg-muted/50 col-start-1 col-end-3 lg:col-end-4 lg:col-start-3">
    <CardHeader className="flex flex-row items-center justify-between space-y-0">
      <Skeleton className="rounded-full h-4 w-24 mb-1" />
      <Skeleton className="rounded-full h-4 w-4" />
    </CardHeader>
    <CardContent>
      <Skeleton className="rounded-full h-8 w-36" />
    </CardContent>
  </Card>
);

// Skeleton component for Monthly Orders Chart
const MonthlyOrdersChartSkeleton = () => {
  const heightValue = (height: number) => {
    if (height === 1) {
      return "h-36"
    }

    if (height === 2) {
      return "h-44"
    }

    if (height === 3) {
      return "h-28"
    }

    if (height === 5) {
      return "h-14"
    }

    if (height === 9) {
      return "h-64";
    }
    if (height === 6 || height === 10 || height === 12) {
      return "h-32";
    }
    if (height === 4 || height === 7 || height === 8 || height === 11) {
      return "h-20";
    }
    return "h-40";
  };

  return (
    <Card className="flex-1 rounded-xl bg-muted/50 col-span-7 lg:col-span-4">
      <CardHeader>
        <CardTitle>Monthly Orders</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="h-[350px] w-full flex items-end gap-2">
          {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12].map((height) => (
            <div key={height} className="flex flex-col items-center flex-1 gap-3">
              <Skeleton className={`w-full ${heightValue(height)}`} />
              <Skeleton className={`w-[75%] h-[10px]`} />
            </div>
          ))}
        </div>
      </CardContent>
      <CardFooter className="flex-col items-start gap-2 text-sm">
        <Skeleton className="h-4 w-48 mb-2" />
        <Skeleton className="h-3 w-36" />
      </CardFooter>
    </Card>
  );
};

// Skeleton component for Recent Sales
const RecentSalesSkeleton = () => (
  <Card className="rounded-xl border bg-muted/50 text-card-foreground shadow col-span-7 lg:col-span-3">
    <CardHeader>
      <CardTitle>Recent Sales</CardTitle>
    </CardHeader>
    <CardContent>
      <div className="space-y-9">
        {[1, 2, 3, 4, 5, 6].map((_, index) => (
          <div key={index} className="flex items-center">
            <Skeleton className="h-9 w-9 rounded-full" />
            <div className="ml-4 space-y-1 flex-grow">
              <Skeleton className="h-4 w-32 mb-2" />
              <Skeleton className="h-3 w-24" />
            </div>
            <Skeleton className="h-4 w-20 ml-auto" />
          </div>
        ))}
      </div>
    </CardContent>
  </Card>
);

export default function Home() {
  const [chartData, setChartData] = useState<
    { month: string; orders: number }[]
  >([]);
  const [orders, setOrders] = useState<Order[]>([]);
  const [totalRevenue, setTotalRevenue] = useState(0);
  const [totalProfits, setTotalProfits] = useState(0);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        setIsLoading(true);
        const response = await fetch("http://localhost:5202/api/Order");
        const data = await response.json();
        setOrders(data.$values);

        const revenue = data.$values.reduce(
          (acc: number, order: any) => acc + order.total + order.taxes,
          0
        );
        setTotalRevenue(revenue);

        const profits = data.$values.reduce(
          (acc: number, order: any) => acc + (order.total + order.taxes) * 0.3,
          0
        );
        setTotalProfits(profits);

        const monthlyOrders = data.$values.reduce(
          (acc: { [key: string]: number }, order: any) => {
            const date = new Date(order.dateCreated);
            const monthKey = date.toLocaleString("default", { month: "long" });

            acc[monthKey] = (acc[monthKey] || 0) + 1;
            return acc;
          },
          {}
        );

        const chartDataFormat = ALL_MONTHS.map((month) => ({
          month,
          orders: monthlyOrders[month] || 0,
        }));

        setChartData(chartDataFormat);
        setIsLoading(false);
      } catch (error) {
        console.error("Error fetching orders:", error);
        setIsLoading(false);
      }
    };

    fetchOrders();
  }, []);

  if (isLoading) {
    return (
      <div className="flex flex-1 flex-col gap-4 p-4 pt-0">
        <h2 className="text-3xl font-bold tracking-tight">Dashboard</h2>
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          <MetricCardSkeleton />
          <MetricCardSkeleton />
          <MetricThirdCardSkeleton />
        </div>
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
          <MonthlyOrdersChartSkeleton />
          <RecentSalesSkeleton />
        </div>
      </div>
    );
  }

  return (
    <div className="flex flex-1 flex-col gap-4 p-4 pt-0">
      <h2 className="text-3xl font-bold tracking-tight">Dashboard</h2>
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        <Card className="rounded-xl bg-muted/50">
          <CardHeader className="flex flex-row items-center justify-between space-y-0">
            <CardDescription>Total Revenue</CardDescription>
            <DollarSign className="text-muted-foreground w-4 h-4" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {new Intl.NumberFormat("en-US", {
                style: "currency",
                currency: "USD",
              }).format(totalRevenue)}
            </div>
          </CardContent>
        </Card>
        <Card className="rounded-xl bg-muted/50">
          <CardHeader className="flex flex-row items-center justify-between space-y-0">
            <CardDescription>Total Profits</CardDescription>
            <DollarSign className="text-muted-foreground w-4 h-4" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {new Intl.NumberFormat("en-US", {
                style: "currency",
                currency: "USD",
              }).format(totalProfits)}
            </div>
          </CardContent>
        </Card>
        <Card className="rounded-xl bg-muted/50 grid col-start-1 col-end-3 lg:col-end-4 lg:col-start-3">
          <CardHeader className="flex flex-row items-center justify-between space-y-0">
            <CardDescription>Number of Sales</CardDescription>
            <CreditCard className="text-muted-foreground w-4 h-4" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{orders.length}</div>
          </CardContent>
        </Card>
      </div>
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
        <Card className="flex-1 rounded-xl bg-muted/50 col-span-7 lg:col-span-4">
          <CardHeader>
            <CardTitle>Monthly Orders</CardTitle>
          </CardHeader>
          <CardContent>
            <ChartContainer config={chartConfig}>
              <BarChart
                accessibilityLayer
                data={chartData}
                margin={{
                  top: 20,
                }}
              >
                <CartesianGrid vertical={false} />
                <XAxis
                  dataKey="month"
                  tickLine={false}
                  tickMargin={10}
                  axisLine={false}
                  tickFormatter={(value) => value.slice(0, 3)}
                />
                <ChartTooltip
                  cursor={false}
                  content={<ChartTooltipContent hideLabel />}
                />
                <Bar dataKey="orders" fill="var(--color-desktop)" radius={8}>
                  <LabelList
                    position="top"
                    offset={12}
                    className="fill-foreground"
                    fontSize={12}
                  />
                </Bar>
              </BarChart>
            </ChartContainer>
          </CardContent>
          <CardFooter className="flex-col items-start gap-2 text-sm">
            <div className="flex gap-2 font-medium leading-none">
              Showing total orders per month <TrendingUp className="h-4 w-4" />
            </div>
            <div className="leading-none text-muted-foreground">
              Based on the latest order data
            </div>
          </CardFooter>
        </Card>
        <Card className="rounded-xl border bg-muted/50 text-card-foreground shadow col-span-7 lg:col-span-3">
          <CardHeader>
            <CardTitle>Recent Sales</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-9">
              {orders
                .sort((a, b) => {
                  return (
                    new Date(b.dateCreated).getTime() -
                    new Date(a.dateCreated).getTime()
                  );
                })
                .slice(0, 6)
                .map((order, index) => (
                  <div key={index} className="flex items-center">
                    <Avatar className="rounded-full bg-muted h-9 w-9 flex items-center justify-center">
                      <AvatarFallback className="text-sm">
                        {order.customerName.substring(0, 1).toUpperCase()}
                        {order.customerName
                          .substring(
                            order.customerName.indexOf(" "),
                            order.customerName.indexOf(" ") + 2
                          )
                          .trim()}
                      </AvatarFallback>
                    </Avatar>
                    <div className="ml-4 space-y-1">
                      <p className="text-sm font-medium leading-none truncate">
                        {order.customerName}
                      </p>
                      <p className="text-sm text-muted-foreground truncate">
                        {order.customerEmail}
                      </p>
                    </div>
                    <div className="ml-auto font-medium truncate">
                      +
                      {new Intl.NumberFormat("en-US", {
                        style: "currency",
                        currency: "USD",
                      }).format(order.total + order.taxes)}
                    </div>
                  </div>
                ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
