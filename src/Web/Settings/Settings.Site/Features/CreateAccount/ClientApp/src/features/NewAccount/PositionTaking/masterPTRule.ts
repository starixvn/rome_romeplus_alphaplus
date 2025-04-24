function minUM2(maxN3SubtractA4: number) {
  return Math.max(0, maxN3SubtractA4);
}

function maxUM2(minM3: number) {
  return minM3;
}

function minM2(maxN3SubtractA4: number) {
  return Math.max(0, maxN3SubtractA4);
}

function maxM2(minM3: number) {
  return minM3;
}

function minA2Auto(maxN3: number, uM2: number) {
  return Math.max(0, maxN3 - uM2);
}

function maxA2Auto(minM3: number, minA4: number) {
  return Math.min(minM3, minA4);
}

function minA2(maxN3: number, m2: number) {
  return Math.max(0, maxN3 - m2);
}

function maxA2(minM3: number, m2: number, minA4: number) {
  return Math.min(minM3 - m2, minA4);
}

function minN2() {
  return 0;
}

function maxN2(a2: number) {
  return a2;
}

function getMasterPTByChangeUM2(minA4: number, maxN3: number, minM3: number, a2: number, n2: number, newUM2: number) {
  const newMinA2Auto = minA2Auto(maxN3, newUM2);
  const newMaxA2Auto = maxA2Auto(minM3, minA4);
  let newA2 = a2 > newMaxA2Auto ? newMaxA2Auto : a2;
  newA2 = newA2 < newMinA2Auto ? newMinA2Auto : newA2;

  return {
    uM2: newUM2,
    a2: newA2,
    n2: n2 > newA2 ? newA2 : n2,
  };
}

function getMasterPTByChangeM2(minA4: number, maxN3: number, minM3: number, a2: number, n2: number, newM2: number) {
  const newMinA2 = minA2(maxN3, newM2);
  const newMaxA2 = maxA2(minM3, newM2, minA4);
  let newA2 = a2 > newMaxA2 ? newMaxA2 : a2;
  newA2 = newA2 < newMinA2 ? newMinA2 : newA2;

  return {
    m2: newM2,
    a2: newMaxA2,
    n2: n2 > newA2 ? newA2 : n2,
  };
}

export {
  minUM2,
  maxUM2,
  minM2,
  maxM2,
  minA2Auto,
  maxA2Auto,
  minA2,
  maxA2,
  minN2,
  maxN2,
  getMasterPTByChangeUM2,
  getMasterPTByChangeM2,
};
