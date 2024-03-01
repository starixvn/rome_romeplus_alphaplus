function minUS3(maxN4SubtractM4: number) {
  return Math.max(0, maxN4SubtractM4);
}

function maxUS3(minS4: number) {
  return minS4;
}

function minS3(maxN4SubtractM4: number) {
  return Math.max(0, maxN4SubtractM4);
}

function maxS3(minS4: number) {
  return minS4;
}

function minM3Auto(maxN4: number, uS3: number) {
  return Math.max(0, maxN4 - uS3);
}

function maxM3Auto(minS4: number, minM4: number) {
  return Math.min(minS4, minM4);
}

function minM3(maxN4: number, s3: number) {
  return Math.max(0, maxN4 - s3);
}

function maxM3(minS4: number, s3: number, minM4: number) {
  return Math.min(Math.min(minS4 - s3, minM4));
}

function minN3() {
  return 0;
}

function maxN3(m3: number) {
  return m3;
}

function getPTSettingByChangeUS3(maxN4: number, minM4: number, minS4: number, m3: number, n3: number, newUS3: number) {
  const newMinM3Auto = minM3Auto(maxN4, newUS3);
  const newMaxM3Auto = maxM3Auto(minS4, minM4);
  let newM3Value = m3 > newMaxM3Auto ? newMaxM3Auto : m3;
  newM3Value = newM3Value < newMinM3Auto && newMinM3Auto <= newMaxM3Auto ? newMinM3Auto : newM3Value;

  return {
    uS3: newUS3,
    m3: newM3Value,
    n3: n3 > newM3Value ? newM3Value : n3,
  };
}

function getPTSettingByChangeS3(maxN4: number, minM4: number, minS4: number, m3: number, n3: number, newS3: number) {
  const newMinM3 = minM3(maxN4, newS3);
  const newMaxM3 = maxM3(minS4, newS3, minM4);
  let newM3Value = m3 > newMaxM3 ? newMaxM3 : m3;
  newM3Value = newM3Value < newMinM3 ? newMinM3 : newM3Value;

  return {
    s3: newS3,
    m3: newMaxM3,
    n3: n3 > newM3Value ? newM3Value : n3,
  };
}

export {
  minUS3,
  maxUS3,
  minS3,
  maxS3,
  minM3Auto,
  maxM3Auto,
  minM3,
  maxM3,
  minN3,
  maxN3,
  getPTSettingByChangeUS3,
  getPTSettingByChangeS3,
};
